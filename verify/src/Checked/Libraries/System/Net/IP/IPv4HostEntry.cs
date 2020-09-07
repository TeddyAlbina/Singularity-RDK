///////////////////////////////////////////////////////////////////////////////
//
//  Microsoft Research Singularity
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//

namespace System.Net.IP
{
    public class IPv4HostEntry
    {
        private string[] aliases;
        private IPv4[]   addresses;

        public string[] Aliases
        {
            get { return aliases; }
            set { aliases = value; }
        }

        public IPv4[] AddressList
        {
            get { return addresses; }
            set { addresses = value; }
        }

        public IPv4HostEntry(string[] aliases, IPv4[] addresses) {
            this.aliases = aliases;
            this.addresses = addresses;
        } 
    }
} // namespace System.Net.IP
