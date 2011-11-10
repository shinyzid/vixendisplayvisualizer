namespace E131
{
    using System;
    using VixenModules.Controller.E131;

    /// <summary>
    /// E1.31 DMP Layer
    /// </summary>
    public class E131DMP : E131Base
    {
        public UInt16	flagsLength;		// DMP PDU Flags/Length
        public byte		vector;				// DMP Vector (0x02)
        public byte		addrTypeDataType;	// Address Type / Data Type (0xa1)
        public UInt16	firstPropertyAddr;	// DMX Start At DMP 0 (0x0000)
        public UInt16	addrIncrement;		// Property Size (0x0001)
        public UInt16	propertyValueCnt;	// Property Value Count
        public byte[]	propertyValues;		// Property Values

        public bool		malformed = true;	// malformed packet (length error)

        public const int	PHYBUFFER_BASE		= 10;
        public const int	PDU_BASE			= PHYBUFFER_BASE;

        public const int	FLAGSLENGTH_OFFSET			=   0;
        public const int	VECTOR_OFFSET				=   2;
        public const int	ADDRTYPEDATATYPE_OFFSET		=   3;
        public const int	FIRSTPROPERTYADDR_OFFSET	=   4;
        public const int	ADDRINCREMENT_OFFSET		=   6;
        public const int	PROPERTYVALUECNT_OFFSET		=   8;
        public const int	PROPERTYVALUES_OFFSET		=  10;

        public E131DMP()
        {
        }

        public E131DMP(byte[] values, int offset, int slots)
        {
            flagsLength			= (UInt16) (0x7000 | (PDU_BASE + 1 + slots));
            vector				= 0x02;
            addrTypeDataType	= 0xa1;
            firstPropertyAddr	= 0x0000;
            addrIncrement		= 0x0001;
            propertyValueCnt	= (UInt16) (slots + 1);
            propertyValues		= new byte[slots + 1];
            propertyValues[0]   = 0;
            Array.Copy(values, offset, propertyValues, 1, slots);
        }

        public E131DMP(byte[] bfr, int offset)
        {
            FromBfr(bfr, offset);
        }

        public UInt16 PhyLength
        {
            get
            {
                return (UInt16) (PHYBUFFER_BASE + propertyValueCnt);
            }
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
                byte[]	bfr = new byte[PhyLength];

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
            flagsLength			= Extensions.BfrToUInt16Swapped(bfr, offset + FLAGSLENGTH_OFFSET);
            vector				= bfr[offset + VECTOR_OFFSET];
            addrTypeDataType	= bfr[offset + ADDRTYPEDATATYPE_OFFSET];
            firstPropertyAddr	= Extensions.BfrToUInt16Swapped(bfr, offset + FIRSTPROPERTYADDR_OFFSET);
            addrIncrement		= Extensions.BfrToUInt16Swapped(bfr, offset + ADDRINCREMENT_OFFSET);
            propertyValueCnt	= Extensions.BfrToUInt16Swapped(bfr, offset + PROPERTYVALUECNT_OFFSET);
            propertyValues		= new byte[propertyValueCnt];

            malformed = true;

            Array.Copy(bfr, offset + PROPERTYVALUES_OFFSET, propertyValues, 0, propertyValueCnt);

            malformed = false;
        }

        public void ToBfr(byte[] bfr, int offset)
        {
            Extensions.UInt16ToBfrSwapped(flagsLength, bfr, offset + FLAGSLENGTH_OFFSET);
            bfr[offset + VECTOR_OFFSET] = vector;
            bfr[offset + ADDRTYPEDATATYPE_OFFSET] = addrTypeDataType;
            Extensions.UInt16ToBfrSwapped(firstPropertyAddr, bfr, offset + FIRSTPROPERTYADDR_OFFSET);
            Extensions.UInt16ToBfrSwapped(addrIncrement, bfr, offset + ADDRINCREMENT_OFFSET);
            Extensions.UInt16ToBfrSwapped(propertyValueCnt, bfr, offset + PROPERTYVALUECNT_OFFSET);
            Array.Copy(propertyValues, 0, bfr, offset + PROPERTYVALUES_OFFSET, propertyValueCnt);
        }
    }
}