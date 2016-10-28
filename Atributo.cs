using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Atribute
    {
        string Nombre_Atr, Where;
        int Group_Priority,Order_Priority;

        //Constructor de la clase. Recibe cuatro strings primero el nombre, segundo la condicion where, tercero 
        public Atribute(string nom, string where, string group_by,string order_by)
        {
            this.Nombre_Atr = nom;
            this.Where = where;
            int group;
            int order;
            Int32.TryParse(group_by,out group);
            Int32.TryParse(order_by, out order);
            this.Group_Priority = group;
            this.Order_Priority = order;
        }

        //Metodos set de la clase Atributo
        public void setName(string thing)
        {
            this.Nombre_Atr = thing;
        }

        public void setWhere(string thing)
        {
            this.Where = thing;
        }

        public void setGroup_Priority(string thing)
        {
            int num;
            Int32.TryParse(thing, out num);
            this.Group_Priority = num;
        }
        public void setGroupPriority(int thing)
        {
            this.Group_Priority = thing;
        }

        public void setOrder_Priority(string thing)
        {
            int num;
            Int32.TryParse(thing, out num);
            this.Order_Priority = num;
        }

        public void setOrder_Priority(int thing)
        {
            this.Order_Priority = thing;
        }

        //Metodos get de la clase Atribute
        public string getName()
        {
            return this.Nombre_Atr;
        }

        public string getWhere()
        {
            return this.Where; ;
        }

        public int getGroup_Priority()
        {
            return this.Group_Priority;
        }

        public int getOrder_Priority()
        {
            return this.Order_Priority;
        }

        public static string OrderBy_GroupBy_CMD(List<Atribute> list)
        {
            List<Atribute> group_command = new List<Atribute>();
            foreach (Atribute atr in list)
            {
                if (atr.getGroup_Priority() != 0)
                {
                    group_command.Add(atr);
                }
            }

            string command = "GROUP BY ";
            Atribute current = group_command.ElementAt(0);
            while (group_command.Count != 0)
            {
                foreach (Atribute atr in group_command)
                {
                    if (current.getGroup_Priority() > atr.getGroup_Priority())
                    {
                        current = atr;
                    }
                }

                command += " " + current.getName();
                group_command.Remove(current);

                if (group_command.Count != 0)
                {
                    command += ",";
                    current = group_command.ElementAt(0);

                }
            }
            List<Atribute> order_command = new List<Atribute>();
            foreach (Atribute atr in list)
            {
                if (atr.getOrder_Priority() != 0)
                {
                    order_command.Add(atr);
                }
            }
            
            command += " ORDER BY ";
            current = order_command.ElementAt(0);
            while (order_command.Count != 0)
            {
                foreach (Atribute atr in order_command)
                {
                    if (current.getGroup_Priority() > atr.getGroup_Priority())
                    {
                        current = atr;
                    }
                }
                command += " " + current.getName();
                order_command.Remove(current);
                if (order_command.Count != 0)
                {
                    command += ",";
                    current = order_command.ElementAt(0);
                }
            }

            return command;
        }
        /*
        private static string ConstructCommand(string command,List<Atribute> list)
        {
            Atribute current = list.ElementAt(0);
            while (list.Count != 0)
            {
                foreach (Atribute atr in list)
                {
                    if (current.getGroup_Priority() > atr.getGroup_Priority())
                    {
                        current = atr;
                    }
                }
                
                command += " "+current.getName();
                list.Remove(current);
                
                if (list.Count != 0)
                {
                    command += ",";
                    current = list.ElementAt(0);

                }
            }
            return command;
        }*/
    }
}
