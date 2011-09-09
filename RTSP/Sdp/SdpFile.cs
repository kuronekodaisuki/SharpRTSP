﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace Rtsp.Sdp
{
    public class SdpFile
    {
        private static KeyValuePair<string, string> GetKeyValue(TextReader sdpStream)
        {
            string line = sdpStream.ReadLine();
            string[] parts = line.Split(new char[] { '=' }, 2);
            if (parts.Length != 2)
                throw new InvalidDataException();
            if (parts[0].Length != 1)
                throw new InvalidDataException();
            if (parts[1].Length != 1)
                throw new InvalidDataException();


            KeyValuePair<string, string> value = new KeyValuePair<string, string>(parts[0], parts[1]);
            return value;
        }

        /// <summary>
        /// Reads the specified SDP stream.
        /// As define in RFC 4566
        /// </summary>
        /// <param name="sdpStream">The SDP stream.</param>
        /// <returns></returns>
        public static SdpFile Read(TextReader sdpStream)
        {
            SdpFile returnValue = new SdpFile();
            KeyValuePair<string, string> value = GetKeyValue(sdpStream);

            // Version mandatory
            if (value.Key == "v")
            {
                returnValue.Version = int.Parse(value.Value, CultureInfo.InvariantCulture);
            }
            else
                throw new InvalidDataException();
            value = GetKeyValue(sdpStream);

            // Origin mandatory
            if (value.Key == "o")
            {
                returnValue.Origin = Origin.Parse(value.Value);
            }
            else
                throw new InvalidDataException();
            value = GetKeyValue(sdpStream);

            // Session mandatory
            if (value.Key == "s")
            {
                returnValue.Session = value.Value;
            }
            else
                throw new InvalidDataException();
            value = GetKeyValue(sdpStream);

            // Session optional
            if (value.Key == "i")
            {
                returnValue.SessionInformation = value.Value;
                value = GetKeyValue(sdpStream);
            }

            // Uri optional
            if (value.Key == "u")
            {
                returnValue.Url = new Uri(value.Value);
                value = GetKeyValue(sdpStream);
            }

            // Email optional
            if (value.Key == "e")
            {
                returnValue.Email = value.Value;
                value = GetKeyValue(sdpStream);
            }

            // Phone optional
            if (value.Key == "p")
            {
                returnValue.Phone = value.Value;
                value = GetKeyValue(sdpStream);
            }

            // Connexion optional
            if (value.Key == "c")
            {
                returnValue.Connection = new Connection(value.Value);
                value = GetKeyValue(sdpStream);
            }

            // bandwidth optional
            if (value.Key == "b")
            {
                returnValue.Bandwidth = new Bandwidth(value.Value);
                value = GetKeyValue(sdpStream);
            }

            //Timing
            while (value.Key == "t")
            {
                string timing = value.Value;
                string repeat = string.Empty;
                value = GetKeyValue(sdpStream);
                if (value.Key == "r")
                {
                    repeat = value.Value;
                    value = GetKeyValue(sdpStream);
                }
                returnValue.Timings.Add(new Timing(timing, repeat));
            }

            // timezone optional
            if (value.Key == "z")
            {
                
                returnValue.TimeZone = new SdpTimeZone(value.Value);
                value = GetKeyValue(sdpStream);
            }

            // enkription key optional
            if (value.Key == "k")
            {

                returnValue.EncriptionKey = new EncriptionKey(value.Value);
                value = GetKeyValue(sdpStream);
            }

            //Attribut optional multiple
            while (value.Key == "a")
            {
                returnValue.Attributs.Add(new Attribut(value.Value));
                value = GetKeyValue(sdpStream);
            }

            return returnValue;
        }


        public int Version { get; set; }


        public Origin Origin { get; set; }

        public string Session { get; set; }

        public string SessionInformation { get; set; }

        public Uri Url { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Connection Connection { get; set; }

        public Bandwidth Bandwidth { get; set; }

        private readonly List<Timing> timingList = new List<Timing>();

        public IList<Timing> Timings
        {
            get
            {
                return timingList;
            }
        }

        public SdpTimeZone TimeZone { get; set; }

        public EncriptionKey EncriptionKey { get; set; }

        private readonly List<Attribut> attributs = new List<Attribut>();

        public IList<Attribut> Attributs
        {
            get
            {
                return attributs;
            }
        }
    
    }
}