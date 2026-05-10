namespace MMLAP.Models
{
    public class MMLEnums
    {
        public enum ItemCategory
        {
            Nothing = 0,
            Zenny = 1,
            Buster = 2,
            Special = 3,
            Normal = 4,
            AP = 5,
            Cheat = 6,
        }

        public enum LocationCategory
        {
            Container = 0,
            Hole = 1,
            Combat = 2,
            Quest = 3,
            Pickup = 4,
        }

        public enum CompletionGoal
        {
            JUNO = 0,
            ALL_BOSSES = 1,
        }

        public enum Register : byte
        {
            zero = 0x0,
            at   = 0x1,
            v0   = 0x2,
            v1   = 0x3,
            a0   = 0x4,
            a1   = 0x5,
            a2   = 0x6,
            a3   = 0x7,
            t0   = 0x8,
            t1   = 0x9,
            t2   = 0xA,
            t3   = 0xB,
            t4   = 0xC,
            t5   = 0xD,
            t6   = 0xE,
            t7   = 0xF,
            s0   = 0x10,
            s1   = 0x11,
            s2   = 0x12,
            s3   = 0x13,
            s4   = 0x14,
            s5   = 0x15,
            s6   = 0x16,
            s7   = 0x17,
            t8   = 0x18,
            t9   = 0x19,
            k0   = 0x1A,
            k1   = 0x1B,
            gp   = 0x1C,
            sp   = 0x1D,
            fp   = 0x1E,
            ra   = 0x1F,
        }
    }
}
