namespace E131
{
    using System;
    using VixenModules.Controller.E131;

    /// <summary>
    /// E1.31 Framing Layer.
    /// </summary>
    public class E131Framing : E131Base
    {
        public UInt16	flagsLength;		// PDU Flags/Length
        public UInt32	vector;				// Vector (0x00000002)
        public string	sourceName;			// Source Name
        public byte		priority;			// Data Priority
        public UInt16	_reserved;			// reserved (0)
        public byte		sequenceNumber;		// Packet Sequence Number
        public byte		options;			// Options Flags
        public UInt16	universe;			// Universe Number

        public bool		malformed = true;	// malformed packet (length error)

        public const int	PHYBUFFER_SIZE		= 77;
        public const int	PDU_SIZE			= PHYBUFFER_SIZE;

        // note offsets are byte locations within the layer - not within the packet
	
        public const int	FLAGSLENGTH_OFFSET		=   0;
        public const int	VECTOR_OFFSET			=   2;
        public const int	SOURCENAME_OFFSET		=   6;
        public const int	SOURCENAME_SIZE			=  64;
        public const int	PRIORITY_OFFSET			=  70;
        public const int	_RESERVED_OFFSET		=  71;
        public const int	SEQUENCENUMBER_OFFSET	=  73;
        public const int	OPTIONS_OFFSET			=  74;
        public const int	UNIVERSE_OFFSET			=  75;

        public E131Framing()
        {
        }

        public E131Framing(UInt16 length, string source, byte sequence, UInt16 univ)
        {
            flagsLength		= (UInt16) (0x7000 | length);
            vector			= 0x00000002;
            sourceName		= source;
            priority		= 100;
            _reserved		= 0;
            sequenceNumber	= sequence;
            options			= 0;
            universe		= univ;
        }

        public E131Framing(byte[] bfr, int offset)
        {
            FromBfr(bfr, offset);
        }

        public UInt16	Length
        {
            get
            {
                return (UInt16) (flagsLength & 0x0fff);
            }

            set
            {
                flagsLength = (UInt16) (0x7000 | value);
            }
        }
	
        override public byte[] PhyBuffer
        {
            get
            {
                byte[]	bfr = new byte[PHYBUFFER_SIZE];

                ToBfr(bfr, 0);

                return bfr;
            }

            set
            {
                FromBfr(value, 0);
            }
        }

        public void FromBfr(byte[] bfr, int offset)
        {
            flagsLength		= Extensions.BfrToUInt16Swapped(bfr, offset + FLAGSLENGTH_OFFSET);
            vector			= Extensions.BfrToUInt32Swapped(bfr, offset + VECTOR_OFFSET);
            sourceName		= Extensions.BfrToString(bfr, offset + SOURCENAME_OFFSET, SOURCENAME_SIZE);
            priority		= bfr[offset + PRIORITY_OFFSET];
            _reserved		= Extensions.BfrToUInt16Swapped(bfr, offset + _RESERVED_OFFSET);
            sequenceNumber	= bfr[offset + SEQUENCENUMBER_OFFSET];
            options			= bfr[offset + OPTIONS_OFFSET];
            universe		= Extensions.BfrToUInt16Swapped(bfr, offset + UNIVERSE_OFFSET);

            malformed = true;

            if (Length != bfr.Length - E131Root.PHYBUFFER_SIZE) return;

            malformed = false;
        }

        public void ToBfr(byte[] bfr, int offset)
        {
            Extensions.UInt16ToBfrSwapped(flagsLength, bfr, offset + FLAGSLENGTH_OFFSET);
            Extensions.UInt32ToBfrSwapped(vector, bfr, offset + VECTOR_OFFSET);
            Extensions.StringToBfr(sourceName, bfr, offset + SOURCENAME_OFFSET, SOURCENAME_SIZE);
            bfr[offset + PRIORITY_OFFSET] = priority;
            Extensions.UInt16ToBfrSwapped(_reserved, bfr, offset + _RESERVED_OFFSET);
            bfr[offset + SEQUENCENUMBER_OFFSET] = sequenceNumber;
            bfr[offset + OPTIONS_OFFSET] = options;
            Extensions.UInt16ToBfrSwapped(universe, bfr, offset + UNIVERSE_OFFSET);
        }
    }
}