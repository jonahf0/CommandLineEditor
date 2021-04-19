using System;
using System.IO;
using System.Collections;
using EditorClass;

namespace MainPage
{
    public class MainPageWithStarter
    {
        private string _fileName;
        private Stack _fileForEditor;
        public MainPageWithStarter(string fileName)
        {
            FileName = fileName;
        }

        private string FileName
        {
            get { return _fileName; }

            set { _fileName = value; }
        }

        private Stack FileForEditor
        {
            get { return _fileForEditor; }

            set { _fileForEditor = value; }
        }

        public void Run()
        {
            
        }
    }

    public class MainPageWithoutStarter
    {
        private string _currentCommand;
        private string _currentFileName = null;
        private Stack _currentFileStore = new Stack();
        public MainPageWithoutStarter()
        {

        }

        private string CurrentCommand
        {
            get { return _currentCommand; }
            set { _currentCommand = value; }
        }

        private string CurrentFileName
        {
            get { return _currentFileName; }
            set { _currentFileName = value; }
        }

        private Stack CurrentFileStore
        {
            get { return _currentFileStore; }
            set { _currentFileStore = value; }
        }
        public void Run()
        {
            PrintStartingInfo();
            TakeCommands();
        }

        private void PrintStartingInfo()
        {
            Console.Clear();
            Console.WriteLine("------ Welcome to Command Line Editor! ------");
            Console.WriteLine("Commands: CurrentFile, Clear, Quit, Save, Edit");
            Console.Write(">");
        }

        private void TakeCommands()
        {
            do
            {
                CurrentCommand = Console.ReadLine();

                switch(CurrentCommand.ToLower())
                {
                    case "quit":
                        break;

                    case "edit":
                        RunNewEditor();
                        break;

                    case "save":
                        SaveCurrentFile();
                        break;

                    case "currentfile":
                        PrintCurrentFile();
                        break;

                    case "clear":
                        ClearScreen();
                        break;

                    default:
                        Console.Write("\nCommand not understood, try:\nCurrentFile\nSave\nQuit\nEdit\n\n>");
                        break;
                }

            }
            while (CurrentCommand.ToLower() != "quit" );
            
        }

        private void RunNewEditor()
        {
            Console.Write("\nFile to edit:");
            string tempName = Console.ReadLine();

            if (File.Exists(tempName))
            {
                Console.Clear();

                if (tempName == "")
                {
                    Editor EditorForFile = new Editor(CurrentFileName);
                    CurrentFileStore = EditorForFile.Edit();
                }

                else
                {
                    CurrentFileName = tempName;
                    Editor EditorForFile = new Editor(CurrentFileName);
                    CurrentFileStore = EditorForFile.Edit();
                }
            }
            else
            {
                Console.WriteLine("\nThe file does not exist or could not be found!\n");
            }

            Console.Write(">");
        }

        private  void SaveCurrentFile()
        {
            if (CurrentFileStore.Count == 0)
            {
                Console.Write("\nThere is no file to save!\n\n>");
            }

            else
            {

                Stack CopyStore = (Stack)CurrentFileStore.Clone();
                string fileToSave = "";
                string line;
                using (StreamWriter StreamForFile = new StreamWriter(CurrentFileName))
                {
                    while (CopyStore.Count != 0)
                    {
                        line = (string)CopyStore.Pop();

                        fileToSave = line + "\n" + fileToSave;

                    }

                    StreamForFile.Write(fileToSave);
                }

                Console.Write("\nFile saved!\n\n>");
            }
        }

        private void PrintCurrentFile()
        {
            if (CurrentFileName == null)
            {
                Console.Write("\nThere is no current file in the session, try using Edit\n\n>");
            }

            else 
            {
                Console.Write("\nThe current file is: {0}\n\n>", CurrentFileName);
            }
        }

        private void ClearScreen()
        {
            Console.Clear();
            PrintStartingInfo();
        }
    }
}
