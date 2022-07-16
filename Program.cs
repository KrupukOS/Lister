using System;
using Lister.Class;
using System.IO;

namespace Lister
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string list;
            string item = "x";
            bool IsListMenu = true;
            ItemList currentList = new ItemList("");

            //program loop
            while (true)
            {
                //lists menu loop
                while (IsListMenu)
                {
                    //visualisation of list menu
                    Console.Clear();
                    Console.WriteLine("LISTS");
                    ItemList.ShowAll();
                    Console.WriteLine();
                    Console.WriteLine("1.Create");
                    Console.WriteLine("2.Load");
                    Console.WriteLine("3.Delete");

                    input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                        case "2":
                        case "3":
                            Console.WriteLine("Type the name of the list");
                            list = Console.ReadLine();
                            currentList = new ItemList(list);

                            //list menu options
                            switch (input)
                            {
                                case "1":
                                    IsListMenu = !IsListMenu;
                                    break;
                                case "2":
                                    if (File.Exists(currentList.Path))
                                    {
                                        currentList.Open();
                                        IsListMenu = !IsListMenu;
                                    }
                                    break;
                                case "3":
                                    currentList.Delete();
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }

                //visualisation of item menu
                Header();
                Console.WriteLine();
                Console.WriteLine("1.Add");
                Console.WriteLine("2.Add inside an item");
                Console.WriteLine("3.Remove");
                Console.WriteLine("4.Toggle");
                Console.WriteLine("5.Save");
                Console.WriteLine("6.Exit");
                input = Console.ReadLine();

                //item menu options
                switch (input)
                {
                    case "1":
                        do
                        {
                            Header();
                            Console.WriteLine("What item do you want to add to the list? (x to stop)");
                            item = Console.ReadLine();
                            if (item != "" && item != "x")
                            {
                                currentList.Add(item, "");
                                Console.WriteLine("Item has been added to the list");
                            }
                            else
                                Console.WriteLine("Something went wrong, try again");
                        } while (item != "x");
                        break;

                    case "2":
                        Console.WriteLine("In which item do you want to put it in");
                        input = Console.ReadLine();

                        do
                        {
                            Header();
                            Console.WriteLine($"What item do you want to add to {input}? (x to stop)");
                            item = Console.ReadLine();
                            if (item != "" && item != "x")
                            {
                                currentList.Add(item, input);
                                Console.WriteLine("Item has been added to the list");
                            }
                            else
                                Console.WriteLine("Something went wrong, try again");
                        } while (item != "x");
                        break;

                    case "3":
                        do
                        {
                            Header();
                            Console.WriteLine("What item do you want to remove from the list? (x to stop)");
                            item = Console.ReadLine();
                            if (item != "" && item != "x")
                            {
                                currentList.RemoveItem(item);
                                Console.WriteLine("Item has been removed from the list");
                            }
                            else
                                Console.WriteLine("Something went wrong, try again");
                        } while (item != "x");
                        break;

                    case "4":
                        do
                        {
                            Header();
                            Console.WriteLine("Which item do you want to (un)check? (x to stop)");
                            item = Console.ReadLine();
                            if (item != "" && item != "x")
                            {
                                currentList.Toggle(item);
                                Console.WriteLine("Item has been toggled");
                            }
                            else
                                Console.WriteLine("Something went wrong, try again");
                        } while (item != "x");
                        break;

                    case "5":
                        currentList.Save();
                        Console.WriteLine("List has been saved!");
                        Console.ReadLine();
                        IsListMenu = !IsListMenu;
                        break;
                    case "6":
                        IsListMenu = !IsListMenu;
                        break;

                    default:
                        Console.WriteLine("Wrong input try again");
                        break;
                }


            }
            void Header()
            {
                Console.Clear();
                Console.WriteLine(currentList.Name.ToUpper());
                currentList.ShowItems();
            }
        }
    }
}
