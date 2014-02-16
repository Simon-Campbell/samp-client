using SAMP.Client.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SAMP.Client.Data.Queries.Implementations
{
    public class ServerDetailsQuery : IServerDetailsQuery
    {
        public Server GetDetails(Models.Server server)
        {
            var address = Dns.GetHostAddresses(server.Hostname).FirstOrDefault();

            if (address != null)
            {
                var endPoint = new IPEndPoint(address, server.Port);

                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write("SAMP".ToCharArray());

                    var pieces = address.ToString().Split('.');

                    foreach (var piece in pieces)
                        writer.Write(Convert.ToByte(piece));

                    writer.Write((ushort)server.Port);
                    writer.Write('i');

                    //if (opcode == 'p')
                    //    writer.Write("8493".ToCharArray());

                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        socket.SendTimeout = 10000;
                        socket.ReceiveTimeout = 10000;

                        if (socket.SendTo(stream.ToArray(), endPoint) > 0)
                        {
                            HandleInformationResponse(server, socket, endPoint);
                        }
                    }
                }
            }

            return null;
        }

        private Server HandleInformationResponse(Server server, Socket socket, EndPoint endPoint)
        {
            var buffer = new byte[512];
            var bytesReceived = 0;

            try
            {
                bytesReceived = socket.ReceiveFrom(buffer, ref endPoint);
            }
            catch
            {
                return server;
            }
            finally
            {
                socket.Close();
            }

            if (bytesReceived <= 10)
                return server;

            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                // Read past header chars
                var header = reader.ReadChars(10);

                if (reader.ReadChar() != 'i')
                    return server;

                var results = new 
                { 
                    IsPassworded       = Convert.ToBoolean(reader.ReadByte()),
                    CurrentPlayerCount = (int) reader.ReadInt16(),
                    MaximumPlayerCount = (int) reader.ReadInt16(),
                    Name        = new String(reader.ReadChars(reader.ReadInt32())),
                    Gamemode    = new String(reader.ReadChars(reader.ReadInt32())),
                    Mapname     = new String(reader.ReadChars(reader.ReadInt32()))
                };

                server.Name = results.Name;
            }

            return server;
        }

    }
}
