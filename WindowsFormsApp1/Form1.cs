using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string[] tags;
        Dictionary<string, int> animalValues = new Dictionary<string, int>();
        Dictionary<string, int> buildingValues = new Dictionary<string, int>();
        Dictionary<string, int> machineValues = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "";


            //Dictionaries are built using text files
            string[] machineFiles = File.ReadAllText("machineValues.txt").Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string[] animalFiles = File.ReadAllText("animalValues.txt").Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string[] buildingFiles = File.ReadAllText("buildingValues.txt").Split(new string[] { Environment.NewLine }, StringSplitOptions.None);



            foreach (string line in machineFiles)
            {
                if (line != "")
                {
                    int x = line.IndexOf(':');

                    machineValues.Add(line.Remove(x), Int32.Parse(line.Substring(x + 1)));
                }

            }
            


            foreach (string line in animalFiles)
            {
                if (line != "")
                {
                    int x = line.IndexOf(':');

                    animalValues.Add(line.Remove(x), Int32.Parse(line.Substring(x + 1)));
                }

            }
            



            foreach (string line in buildingFiles)
            {
                if (line != "")
                {
                    int x = line.IndexOf(':');

                    buildingValues.Add(line.Remove(x), Int32.Parse(line.Substring(x + 1)));
                }

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            char[] charsToTrim = new char[] { ',', '.', '\n', '\r', '\"', '-', '(', ')', '[', ']', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '?', '!', ';', ':', '\'' };
            string[] bannedWords = File.ReadAllLines("bannedWords.txt");
            bannedWords.ToHashSet();

            string description = richTextBox1.Text;
            tags = description.Split(' ');
            
            

            int counter = 0;
            foreach (var item in tags)
            {
                tags[counter] = tags[counter].ToLower().Trim(charsToTrim);

                if (bannedWords.Contains(tags[counter]) == true)
                {
                    tags[counter] = "";
                }

                counter++;
            }
            
            string Guess = makeGuess();

            label2.Text = Guess;




        }

        private void writeTags()
        {
            //increases dictionary values with tags
            //writes tags to txt file
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                string type = checkedListBox1.CheckedItems[0].ToString();

                

                if (type == "Animal")
                {
                    foreach (string tag in tags)
                    {



                        if (machineValues.ContainsKey(tag) == true)
                        {
                            
                            machineValues.Remove(tag);
                        }
                        else if (buildingValues.ContainsKey(tag) == true)
                        {
                            
                            buildingValues.Remove(tag);
                        }
                        else
                        {
                            if (animalValues.ContainsKey(tag) == true)
                            {
                                //increment the value for that tag in data structure
                                animalValues[tag]++;
                            }
                            else
                            {
                                //add tag to data structure
                                animalValues.Add(tag, 1);
                            }
                        }

                        if (animalValues.ContainsKey(tag) == true)
                        {
                            //increment the value for that tag in data structure
                            animalValues[tag]++;
                        }
                        else
                        {
                            //add tag to data structure
                            animalValues.Add(tag, 1);
                        }
                    }


                }
                else if (type == "Machine")
                {



                    foreach (string tag in tags)
                    {

                        if (animalValues.ContainsKey(tag) == true)
                        {

                            animalValues.Remove(tag);
                        }
                        else if (buildingValues.ContainsKey(tag) == true)
                        {

                            buildingValues.Remove(tag);
                        }
                        else
                        {
                            if (machineValues.ContainsKey(tag) == true)
                            {
                                machineValues[tag]++;
                            }
                            else
                            {
                                machineValues.Add(tag, 1);
                            }
                        }


                    }


                }
                else if (type == "Building")
                {
                    foreach (string tag in tags)
                    {

                        if (animalValues.ContainsKey(tag) == true)
                        {

                            animalValues.Remove(tag);
                        }
                        else if (machineValues.ContainsKey(tag) == true)
                        {

                            machineValues.Remove(tag);
                        }
                        else
                        {

                            if (buildingValues.ContainsKey(tag) == true)
                            {
                                buildingValues[tag]++;
                            }
                            else
                            {
                                buildingValues.Add(tag, 1);
                            }
                        }

                    }


                }

                writeDictToFile("animalValues.txt", animalValues);
                writeDictToFile("machineValues.txt", machineValues);
                writeDictToFile("buildingValues.txt", buildingValues);

                void writeDictToFile(string fileName, Dictionary<string, int> dict)
                {
                    string[] lines = new string[dict.Count];
                    int counter = 0;
                    foreach (string item in dict.Keys)
                    {

                        if (item != "")
                        {
                            lines[counter] = item + ':' + dict[item];
                            counter++;
                        }
                    }

                    File.WriteAllLines(fileName, lines);
                }
            }

            

            

        }

        private int checkDict(Dictionary<string, int> dict, string Tag)
        {
            int value = dict[Tag];
            return value;
        }
        private string makeGuess()
        {
            string guess = "";



            //calculating values for each tag for each dictionary
            Double animalTotal = 0;
            Double buildingTotal = 0;
            Double machineTotal = 0;

            Double totalAnimalPoints = 0;
            Double totalBuildingPoints = 0;
            Double totalMachinePoints = 0;

            foreach (int item in animalValues.Values)
            {
                totalAnimalPoints += item;
            }

            foreach (int item in buildingValues.Values)
            {
                totalBuildingPoints += item;
            }

            foreach (int item in machineValues.Values)
            {
                totalMachinePoints += item;
            }


            //if any files are null, default values to 1 as so that we don't divide by 0
            if (totalAnimalPoints == 0)
            {
                totalAnimalPoints = 1;
            }
            if (totalBuildingPoints == 0)
            {
                totalBuildingPoints = 1;
            }
            if (totalMachinePoints == 0)
            {
                totalMachinePoints = 1;
            }


            foreach (string tag in tags)
            {

                //
                Double animalValue;
                Double q = 0;
                bool check1 = animalValues.TryGetValue(tag, out int test1);
                if (check1 == true)
                {
                    q = checkDict(animalValues, tag);

                }



                animalValue = q / totalAnimalPoints;
                animalTotal += animalValue;
                //
                Double buildingValue;

                Double p = 0;
                bool check2 = buildingValues.TryGetValue(tag, out int test2);
                if (check2 == true)
                {
                    p = checkDict(buildingValues, tag);
                    
                }
                
                buildingValue = p / totalBuildingPoints;
                    
                buildingTotal += buildingValue;
                //
                Double machineValue;

                Double g = 0;
                bool check3 = machineValues.TryGetValue(tag, out int test3);
                if (check3 == true)
                {
                    g = checkDict(machineValues, tag);

                }

                machineValue = g / totalMachinePoints;

                machineTotal += machineValue;

            }

            if ((machineTotal > animalTotal) & (machineTotal > buildingTotal))
            {
                guess = "Machine";
            }
            else if ((animalTotal > machineTotal) & (animalTotal > buildingTotal))
            {
                guess = "Animal";
            }
            else if ((buildingTotal > animalTotal) & (buildingTotal > machineTotal))
            {
                guess = "Building";
            }
            else
            {
                Random random = new Random();
                int randNum = random.Next(0, 3);
                if (randNum == 0)
                {
                    guess = "Machine";
                }
                if (randNum == 1)
                {
                    guess = "Animal";
                }
                if (randNum == 2)
                {
                    guess = "Building";
                }
            }

            return guess;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            if (e.NewValue == CheckState.Checked)
            {
                //checking a box
                
            }
            else
            {
                //unchecking a box
                
                return;
            }
            
            int checkedIndex = e.Index;
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                //no boxes are checked
                return;
            }
            else
            {
                //a box is already checked
                e.NewValue = CheckState.Unchecked;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeTags();
            clearForm();

        }

        private void clearForm()
        {
            richTextBox1.Text = "";
            checkedListBox1.ClearSelected();
            label2.Text = "";
            tags = new string[] { };

        }
    }
}
