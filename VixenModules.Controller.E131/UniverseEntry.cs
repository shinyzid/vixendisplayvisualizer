//-----------------------------------------------------------------
//
//	UniverseEntry - a class to keep all in memory info together
//
//-----------------------------------------------------------------
namespace VixenModules.Controller.E131
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class UniverseEntry
    {
        // our row # (1-x) for message texts

        public	int		rowNum;				// row # (1-x)

        // these are the fields from the configuration

        public	bool	active;				// is it active
        public	int		universe;			// universe number
        public	int		start;				// starting slot within events (zero based!!!)
        public	int		size;				// slot count
        public	string	unicast;			// unicast ip addr (if not null)
        public	string	multicast;			// multicast nic id (if not null)
        public	int		ttl;				// time to live

        // these are dynamic fields for processing events

        public	Socket		socket;				// socket to use
        public	IPEndPoint	destIPEndPoint;		// destination end point
        public	byte[]		phyBuffer;			// physical buffer
        public	int			eventRepeatCount;	// how many identical pkts to skip (0 = none)

        // these are the per universe statistics

        public	int			pktCount;			// packet count
        public	Int64		slotCount;			// slot count

        public UniverseEntry(int rowNum, bool active, int universe, int start, int size, string unicast, string multicast, int ttl)
        {
            this.rowNum		= rowNum;
            this.active		= active;
            this.universe	= universe;
            this.start		= start;
            this.size		= size;
            this.unicast	= unicast;
            this.multicast	= multicast;
            this.ttl		= ttl;

            this.socket				= null;
            this.destIPEndPoint		= null;
            this.phyBuffer			= null;
            this.eventRepeatCount	= 0;

            this.pktCount			= 0;
            this.slotCount			= 0;
        }

        public string RowUnivToText
        {
            get
            {
                StringBuilder	text = new StringBuilder();

                text.Append("Row ");
                text.Append(rowNum.ToString());
                text.Append(":");
                text.Append(" Univ=");
                text.Append(universe.ToString());
                return text.ToString();
            }
        }

        public string InfoToText
        {
            get
            {
                StringBuilder	text = new StringBuilder();

                text.Append("Row ");
                text.Append(rowNum.ToString());
                text.Append(":");
                text.Append(" Univ=");
                text.Append(universe.ToString());
                text.Append(" Start=");
                text.Append((start+1).ToString());
                text.Append(" Size=");
                text.Append(size.ToString());
                if (unicast != null) text.Append(" Unicast");
                if (multicast != null) text.Append(" Multicast");
                text.Append(" TTL=");
                text.Append(ttl.ToString());
                return text.ToString();
            }
        }

        public string StatsToText
        {
            get
            {
                StringBuilder	text = new StringBuilder();

                text.Append("Row ");
                text.Append(rowNum.ToString());
                text.Append(":");
                text.Append(" Univ=");
                text.Append(universe.ToString());
                text.Append("  Packets=");
                text.Append(pktCount.ToString());
                text.Append("  Slots=");
                text.Append(slotCount.ToString());
                return text.ToString();
            }
        }
    }
}