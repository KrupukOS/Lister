using System;
using System.Collections.Generic;
using System.IO;

namespace Lister.Class
{
    public class ItemList
    {
        private static string mainPath = "./../../../";
        public List<Item> list = new List<Item>();
        public string Name { get; set; }
        public string Path { get; set; }
        public ItemList(string name)

        {
            Name = name.ToLower();
            Path = $"./../../../{name}.txt";
        }

        //show all existing lists
        public static void ShowAll()
        {
            string[] lists = Directory.GetFiles(mainPath, "*.txt*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < lists.Length; i++)
            {
                lists[i] = lists[i].Replace(".txt", "");
                lists[i] = lists[i].Replace(mainPath, "");
                Console.WriteLine(lists[i]);
            }
        }

        //open existing list
        public void Open()
        {
            if (File.Exists(Path))
            {
                StreamReader sr = new StreamReader(Path);
                int amount = Convert.ToInt32(sr.ReadLine());

                RemoveAllItems();
                for (int i = 0; i < amount; i++)
                {
                    string[] iteminfo = sr.ReadLine().Split(",", 3, StringSplitOptions.RemoveEmptyEntries);

                    Add(iteminfo[0], iteminfo[1]);
                    if (Convert.ToBoolean(iteminfo[2]) == true)
                    {
                        Toggle(iteminfo[0]);
                    }
                }
                sr.Close();
            }
        }

        //save current list
        public void Save()
        {
            if (!File.Exists(Path))
            {
                //create file
                File.Create(Path).Close();
            }
            else
            {
                //clear file
                File.WriteAllText(Path, "");
            }
            //write to file
            StreamWriter sw = new StreamWriter(Path);
            sw.WriteLine(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Parent != null)
                {
                    sw.WriteLine($"{list[i].Name},{list[i].Parent.Name},{list[i].Packed}");
                }
                else
                {
                    sw.WriteLine($"{list[i].Name},null,{list[i].Packed}");
                }
            }
            
            sw.Close();
        }

        //delete an existing list
        public void Delete()
        {
            File.Delete(Path);
        }

        //add an item to the list
        public void Add(string item, string parent)
        {
            list.Add(new Item() { Name = item, Parent = GetItem(parent) });
        }

        //remove an item from the list
        public void RemoveItem(string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    list.Remove(list[i]);
            }
        }

        public void RemoveAllItems()
        {
            for (int i = 0; i < list.Count; i++)
            {
                    list.Remove(list[0]);
            }
        }

        public Item GetItem(string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    return list[i];
            }
            return null;
        }

        //get the amount of layers
        public int GetAmountOfLayers()
        {
            int highestNumber = 0;
            int currentNumber = 0;
            Item currentItem;

            for (int i = 0; i < list.Count; i++)
            {
                currentItem = list[i];
                while (currentItem.Parent != null)
                {
                    currentItem = currentItem.Parent;
                    currentNumber++;
                    if (currentNumber > highestNumber)
                    {
                        highestNumber = currentNumber;
                        currentNumber = 0;
                    }
                }
            }
            return highestNumber;
        }
        //find the layer of a specific item
        private int GetLayerIndex(Item currentItem)
        {
            int currentNumber = 0;
            while (currentItem.Parent != null)
            {
                currentItem = currentItem.Parent;
                currentNumber++;
            }
            return currentNumber;
        }

        //shows all list items
        public void ShowItems()
        {
            //primary layer
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Parent == null)
                {
                    if (list[i].Packed == true)
                        Console.Write("[X] ");
                    else
                        Console.Write("[ ] ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    //every item with an item inside will be colored
                    for (int k = 0; k < list.Count; k++)
                    {
                        if (list[i] == list[k].Parent)
                            Console.ForegroundColor = ConsoleColor.Yellow;

                    }
                    Console.WriteLine(list[i].Name);
                    Console.ResetColor();

                    //all secondary layers
                    ShowItemRecursion(i);
                }
            }
        }

        //part of the show method
        private void ShowItemRecursion(int i)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Parent == list[i])
                {
                    int layer = GetLayerIndex(list[i]);

                    if (list[j].Packed == true)
                        Console.Write("[X] ");
                    else
                        Console.Write("[ ] ");

                    for (int x = 0; x < layer; x++)
                        Console.Write("    ");


                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    //every item with an item inside will be colored
                    for (int k = 0; k < list.Count; k++)
                    {
                        if (list[j] == list[k].Parent)
                            Console.ForegroundColor = ConsoleColor.Blue;

                    }
                    Console.Write(" └- ");
                    Console.WriteLine(list[j].Name);
                    Console.ResetColor();

                    ShowItemRecursion(j);
                }

            }
        }

        //toggle packed state of item
        public void Toggle(string name)
        {
            Item item = GetItem(name);
            if (item != null)
            {
                item.Packed = !item.Packed;
                ToggleRecursion(name, item.Packed);
            }
        }

        //part of the toggle method
        private void ToggleRecursion(string name, bool isPacked)
        {
            Item item = GetItem(name);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Parent == item)
                {
                    list[i].Packed = isPacked;
                    ToggleRecursion(list[i].Name, isPacked);
                }
            }
        }

    }
}
