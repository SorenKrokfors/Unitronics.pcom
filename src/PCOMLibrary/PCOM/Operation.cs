using System;
using System.Collections.Generic;
using System.IO;

namespace Unitronics.PCOM
{
    /*
    Operand Type	    Enumeration Value	Size in byte per Value
    MB	                1	                1	
    SB	                2	                1
    MI	                3	                2
    SI	                4	                2
    ML	                5	                4
    SL	                6	                4
    MF	                7	                4
    SF	                8	                4
    Input           	9	                1
    Output	            10	                1
    Timer Run Bit	    11	                1
    Counter Run Bit	    12	                1
    DW	                16                  4
    SDW	                36	                4
    Timer Preset	    128	                4
    Timer Current	    129	                4
    Counter Current	    145	                2
    Counter Preset	    144	                2
    XB	                64	                1
    XI	                65	                2
    XL	                66	                4
    XDW	                67	                4
     */

    public enum OperationType:byte
    {
        MB = 1,
        SB = 2,
        MI = 3,
        SI = 4,
        ML = 5,
        SL = 6,
        MF = 7,
        SF = 8,
        Input = 9,
        Output = 10,
        TimerRunBit = 11,
        CounterRunBit = 12,
        DW = 16,
        SDW = 36,
        TimerPreset = 128,
        TimerCurrent = 129,
        CounterCurrent = 145,
        CounterPreset = 144,
        XB = 64,
        XI = 65,
        XL = 66,
        XDW = 67,
    }

    public abstract class Operation<T> : AbstractOperation
    {
        protected IList<T> Values { get; set; } = new List<T>();

        public virtual void AddValue(T value)
        {
            Values.Add(value);
        }

        public IList<T> GetValues()
        {
            return Values;
        }

        /// <summary>
        /// TODO: write to List<byte>
        /// </summary>
        /// <param name="data"></param>
        protected void FillHeader(MemoryStream data)
        {
            if (Values.Count == 0) throw new ArgumentOutOfRangeException("Empty list value");
            data.WriteByte((byte)OperationType);
            data.WriteByte(NumberOfOperands);
            var bytes = BitConverter.GetBytes(StartAddress);
            data.Write(bytes, 0, bytes.Length);
        }

        public override string GetTextValue()
        {
            return string.Join(",", Values);
        }

        public override void Clear()
        {
            Values.Clear();
        }
    }
}