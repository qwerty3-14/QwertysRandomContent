using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QwertysRandomContent
{
    public class Deck<F> : List<F>
    {
        //This class is inspired and tries to simulate drawing from a deck of cards
        
        
        void fillIndexes(ref int[] Indexes, int count)
        {
            for (int n = 0; n < count; n++)
            {
                Indexes[n] = Main.rand.Next(this.Count - n);
                for (int j = 0; j < n; j++)
                {
                    if (Indexes[n] >= Indexes[j])
                    {
                        Indexes[n]++;
                    }
                }
                Array.Sort(Indexes);
            }
        }
        public F[] Draw(int count)
        {
            
            
            int[] Indexes = new int[count];
            F[] selection = new F[count];
            if(count ==1)
            {
                fillIndexes(ref Indexes, count);
            }
            else
            {
                while (Indexes[0] == Indexes[1])//I couldn't figure out why on rare occasion you can get 2 of the first index so this just hard checks and forces a redo
                {
                    fillIndexes(ref Indexes, count);
                }
            }
            
            
            for (int i = 0; i < Indexes.Length; i++)
            {
                selection[i] = this[Indexes[i]];
            }
            
            return selection;

        }
        public void Shuffle()
        {
            Deck<F> g = new Deck<F>();
            while(Count>0)
            {
                int r = Main.rand.Next(Count);
                g.Add(this[r]);
                RemoveAt(r);
            }
            foreach(F item in g)
            {
                Add(item);
            }
        }
    }
}
