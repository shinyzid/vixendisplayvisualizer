namespace VixenModules.Controller.E131
{
    using System;
    using global::E131;

    /// <summary>
    ///   The E1.31 packet.
    /// </summary>
    public class E131Pkt : E131Base
    {
        public E131DMP e131DMP;
        public E131Framing e131Framing;
        public E131Root e131Root;

        private const int DMP_OFFSET = E131Root.PHYBUFFER_SIZE + E131Framing.PHYBUFFER_SIZE;
        private const int FRAMING_OFFSET = E131Root.PHYBUFFER_SIZE;
        private const int ROOT_OFFSET = 0;

        public E131Pkt() {}

        public E131Pkt(Guid guid, string source, byte sequence, ushort universe, byte[] values, int offset, int slots)
        {
            e131DMP = new E131DMP(values, offset, slots);
            e131Framing = new E131Framing((UInt16)(E131Framing.PHYBUFFER_SIZE + e131DMP.Length), source, sequence, universe);
            e131Root = new E131Root((UInt16)(E131Root.PDU_SIZE + e131Framing.Length), guid);
        }

        public E131Pkt(byte[] bfr)
        {
            PhyBuffer = bfr;
        }

        public override byte[] PhyBuffer
        {
            get
            {
                var bfr = new byte[PhyLength];

                e131Root.ToBuffer(bfr, ROOT_OFFSET);
                e131Framing.ToBfr(bfr, FRAMING_OFFSET);
                e131DMP.ToBfr(bfr, DMP_OFFSET);

                return bfr;
            }

            set
            {
                if (value.Length
                    < E131Root.PHYBUFFER_SIZE + E131Framing.PHYBUFFER_SIZE + E131DMP.PHYBUFFER_BASE)
                {
                    return;
                }

                e131Root = new E131Root(value, ROOT_OFFSET);
                if (e131Root.IsMalformed)
                {
                    return;
                }

                e131Framing = new E131Framing(value, FRAMING_OFFSET);
                if (e131Root.IsMalformed)
                {
                    return;
                }

                e131DMP = new E131DMP(value, DMP_OFFSET);
                if (e131DMP.malformed)
                {
                    return;
                }
            }
        }

        public ushort PhyLength
        {
            get
            {
                return (UInt16)(E131Root.PHYBUFFER_SIZE + e131Framing.Length);
            }
        }

        // -------------------------------------------------------------
        // 	CompareSlots() - compare a new event buffer against current
        // 					 slots
        // 		this is a static function to work on prebuilt packets.
        // 		it is embedded in the E131Pkt class to keep it with
        // 		the constants and rules that were used to build the
        // 		original packet.
        // -------------------------------------------------------------

        public static bool CompareSlots(byte[] phyBuffer, byte[] values, int offset, int slots)
        {
            var idx = E131Root.PHYBUFFER_SIZE + E131Framing.PHYBUFFER_SIZE + E131DMP.PROPERTYVALUES_OFFSET + 1;

            while (slots-- > 0)
            {
                if (phyBuffer[idx++]
                    != values[offset++])
                {
                    return false;
                }
            }

            return true;
        }

        // -------------------------------------------------------------
        // 	CopySlotsSeqNum() - copy a new sequence # and slots into
        // 						an existing packet buffer
        // 		this is a static function to work on prebuilt packets.
        // 		it is embedded in the E131Pkt class to keep it with
        // 		the constants and rules that were used to build the
        // 		original packet.
        // -------------------------------------------------------------

        public static void CopySeqNumSlots(byte[] phyBuffer, byte[] values, int offset, int slots, byte seqNum)
        {
            var idx = E131Root.PHYBUFFER_SIZE + E131Framing.PHYBUFFER_SIZE + E131DMP.PROPERTYVALUES_OFFSET + 1;

            Array.Copy(values, offset, phyBuffer, idx, slots);
            phyBuffer[E131Root.PHYBUFFER_SIZE + E131Framing.SEQUENCENUMBER_OFFSET] = seqNum;
        }
    }
}
