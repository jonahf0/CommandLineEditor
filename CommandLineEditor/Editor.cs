using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EditorClass
{
    class Editor
    {
        private string _fileToEdit;
        private Stack _fileStore;
        private Queue<ConsoleKeyInfo> _keyQueue = new Queue<ConsoleKeyInfo>();
        public Editor(string fileName)
        {
            FileToEdit = fileName;
        }

        private Queue<ConsoleKeyInfo> KeyQueue
        {
            get { return _keyQueue; }
            set { _keyQueue = value; }
        }
        private string FileToEdit
        {
            get { return _fileToEdit; }
            set { _fileToEdit = value; }
        }

        private Stack FileStore
        {
            get { return _fileStore; }
            set { _fileStore = value; }
        }

        public Stack Edit()
        {
            FileStore = OpenFile(FileToEdit);
            FileStore = EditFile();

            Stack theCopy = (Stack)FileStore.Clone();

            while (theCopy.Count != 0)
            {

                Console.WriteLine(theCopy.Pop());

            }

            return FileStore;
        }

        private Stack EditFile()
        {
            Stack TempEditingStore = FileStore;

            PrintFileForEditing();

            ProcessKeyboardInput();

            return TempEditingStore;
        }

        private Stack OpenFile(string fileName)
        {
            Stack tempFileStore = new Stack();

            StreamReader OpenFileForReading = new StreamReader(fileName);

            string line;

            while( (line = OpenFileForReading.ReadLine()) != null)
            {
                line = line.Replace("\t", "    " );
                tempFileStore.Push(line);
            }

            OpenFileForReading.Close();

            return tempFileStore;
        }

        private void PrintFileForEditing()
        {
            Stack temp = (Stack)FileStore.Clone();

            string fileToPrint = "";
            while( temp.Count != 0 )
            {
                fileToPrint = temp.Pop() + "\n" + fileToPrint;
            }
            Console.Write(fileToPrint);

            if (FileStore.Count > 0)
            { 
                Console.SetCursorPosition(((string)FileStore.Peek()).Length, FileStore.Count - 1); 
            }
        }

        private void ProcessKeyboardInput()
        {

            Task TaskOne = new Task(ThreadForProcessingInput);
            Task TaskTwo = new Task(ThreadForTakingInput);

            TaskOne.Start();
            TaskTwo.Start();

            TaskOne.Wait();
            TaskTwo.Wait();

        }

        private void ThreadForProcessingInput()
        {
            ConsoleKeyInfo keyPress = new ConsoleKeyInfo();
            string line;

            do
            {
                if (KeyQueue.Count >= 1)
                {

                    keyPress = KeyQueue.Dequeue();

                    switch (keyPress.Key)
                    {
                        case ConsoleKey.Backspace:

                            if (FileStore.Count > 1 && (string)FileStore.Peek() == "")
                            {
                                FileStore.Pop();

                                Console.SetCursorPosition(((string)FileStore.Peek()).Length, Console.CursorTop - 1);
                            }

                            else if (Console.CursorLeft == 0 && Console.CursorTop == 0)
                            {
                                Console.Write("");
                                Console.CursorLeft = 0;
                                
                            }

                            else
                            {
                                line = (string)FileStore.Pop();
                                line = line.Substring(0, line.Length - 1);
                                FileStore.Push(line);

                                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                                Console.Write(" ");
                                Console.SetCursorPosition(Console.CursorLeft-1, Console.CursorTop);
                            }

                            break;

                        case ConsoleKey.Enter:

                            FileStore.Push("");
                            Console.Write('\n');

                            break;

                        case ConsoleKey.Escape:
                            break;

                        default:

                            if (FileStore.Count == 0)
                            {
                                line = "" + keyPress.KeyChar;
                                FileStore.Push(line);

                                Console.Write(keyPress.KeyChar);
                            }

                            else
                            {
                                line = (string)FileStore.Pop();
                                line = line + keyPress.KeyChar;
                                FileStore.Push(line);
                            }

                            break;
                    }

                }
            }
            while (keyPress.Key != ConsoleKey.Escape);
        }

        private void ThreadForTakingInput()
        {
            ConsoleKeyInfo keyPress;

            do
            {
                keyPress = Console.ReadKey();
                
                KeyQueue.Enqueue(keyPress);

            }
            while (keyPress.KeyChar != (char)27);

            Console.Write("\n\n>");
        }
    }
}
