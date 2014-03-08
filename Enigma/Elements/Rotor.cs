using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enigma.Elements
{
    /// <summary>
    /// Set that cycles. If count is i, then rotor[i] = rotor[0]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Rotor
    {
        int count;
        Connection[] array;
        Connection[] originalArray;

        public int Length { get { return count; } }

        public Connection this[int index]
        {
            get { return array[index % count]; }
        }

        /// <summary>
        /// Rotates the rotor of one increment to the right.
        /// </summary>
        public void Step()
        {
            // Rebuilds the array
            Connection[] steppedArray = new Connection[count];

            // The stepped array has now one slot of difference with the original one
            for (int i = 1; i < count; i++)
            {
                steppedArray[i - 1] = array[i];
            }

            // put the last value of original array into the first slot.
            steppedArray[0] = array[count - 1];
        }

        /// <summary>
        /// Reset the rotor to its original position.
        /// </summary>
        public void Reset()
        {
            array = originalArray;
        }

        public Rotor(Connection[] connections)
        {
            this.array = connections;
            this.originalArray = connections;
            this.count = connections.Length;
        }
    }
}
