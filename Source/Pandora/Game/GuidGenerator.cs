using System.Collections.Generic;

namespace Pandora.Game
{
    public class GuidGenerator
    {
        private uint counter;
        private readonly Queue<uint> reuseQueue = new Queue<uint>();

        private readonly object mutex = new object();

        public GuidGenerator(uint start) { counter = start; }

        public uint Obtain()
        {
            lock (mutex)
                return reuseQueue.Count != 0 ? reuseQueue.Dequeue() : counter++;
        }

        public void Release(uint guid)
        {
            lock (mutex)
                reuseQueue.Enqueue(guid);
        }
    }
}
