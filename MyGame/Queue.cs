using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class TriData
    {
        public int l, c, v;

        public TriData(int l, int c, int v)
        {
            this.l = l;
            this.c = c;
            this.v = v;
        }
        public string View()
        {
            return "(" + l + " " + c + " " + v + ")";
        }
    }

    public class Queue
    {
        int buff;
        TriData[] v;
        int idx;

        public Queue()
        {
            buff = 50;
            v = new TriData[buff];
            idx = 0;
        }

        public void Push(TriData x)
        {
            if (idx == v.Length)
            {
                TriData[] t = new TriData[v.Length + buff];
                for (int i = 0; i < idx; i++)
                {
                    t[i] = v[i];
                }
                t[idx] = x;
                idx++;
                v = t;
            }
            else
            {
                v[idx] = x;
                idx++;
            }
        }

        public TriData Pop()
        {
            TriData tor = v[0];
            if ((v.Length - idx) == buff)
            {
                TriData[] t = new TriData[v.Length - buff + 1];
                for (int i = 0; i < idx - 1; i++)
                {
                    t[i] = v[i + 1];
                }
                idx--;
                v = t;
            }
            else
            {
                for (int i = 0; i < idx - 1; i++)
                {
                    v[i] = v[i + 1];
                }
                idx--;
            }
            return tor;
        }

        public string view()
        {
            string t = "";
            for (int i = 0; i < idx; i++)
            {
                t += v[i].View();
            }
            return t;
        }

        public bool IsEmpty()
        {
            if (idx == 0)
                return true;
            return false;
        }
    }
}
