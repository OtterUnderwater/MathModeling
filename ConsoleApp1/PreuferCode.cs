using ConsoleApp1.models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    internal class PreuferCode
    {
        /// <summary>
        /// Метод, кодирующий дерево в код Прюфера
        /// </summary>
        /// <param name="codeList"></param>
        public void GetPreuferCode(List<int[]> codeList)
        {
            string codeAnswer = ""; //Итоговый код
            Dictionary<int, TreeNode> tree = new Dictionary<int, TreeNode>();
            foreach (int[] rib in codeList)
            {
                int parent = rib[0];
                int child = rib[1];
                // Создаем родителя, если его нет
                if (!tree.ContainsKey(parent))
                {
                    tree[parent] = new TreeNode(parent);
                }
                // Создаем ребенка, если его нет
                if (!tree.ContainsKey(child))
                {
                    tree[child] = new TreeNode(child);
                }
                // добавляет дочерний узел к родительскому
                tree[parent].AddChildren(tree[child]);
            }
            Console.WriteLine("Дерево:");
            PrintTreeConsole(tree[1], 0);
            while (tree.Count > 2)
            {
                // Получаем все листы
                List<TreeNode> leaf = tree.Values.Where(node => node.Children.Count == 0 || (node.Children.Count == 1 && node.Parent == null)).ToList();
                TreeNode minLeaf = leaf.OrderBy(x => x.Value).First(); // находим минимальный лист
                // добавляем связанный элемент в код и удаляем лист
                if (minLeaf.Parent == null) // Проверяем, является ли minLeaf корневым узлом
                {
                    codeAnswer += minLeaf.Children[0].Value + " ";
                    tree.Remove(minLeaf.Value);
                }
                else
                {
                    codeAnswer += minLeaf.Parent.Value + " ";
                    TreeNode parent = tree[minLeaf.Parent.Value]; // нашли родителя
                    parent.Children.Remove(minLeaf);  // удаление minLeaf из списка потомков
                    tree.Remove(minLeaf.Value);
                }
            }
            codeAnswer = codeAnswer.Substring(0, codeAnswer.Length - 1);
            Console.WriteLine("Код прюфера: " + codeAnswer);
            WriteToFileCode("Код прюфера: " + codeAnswer);
        }

        /// <summary>
        /// Декодирование кода в дерево
        /// </summary>
        /// <param name="listCode"></param>
        public void GetTreePreufer(List<int> listCode)
        {
            List<int> listTop = new List<int>();
            List<int[]> ribsList = new List<int[]>();
            for (int i = 1; i <= listCode.Count + 2; i++)
            {
                listTop.Add(i);
            }
            while (listTop.Count > 2)
            {
                int minDistingTop = listTop.FirstOrDefault(t => !listCode.Any(c => t == c));
                int indexMinTop = listTop.IndexOf(minDistingTop);
                int[] rib = { listCode[0], listTop[indexMinTop] };
                ribsList.Add(rib);
                listCode.RemoveAt(0);
                listTop.RemoveAt(indexMinTop);
            }
            int[] endRib = { listTop[0], listTop[1] };
            ribsList.Add(endRib);
            WriteToFileAndConsoleTree(ribsList);
        }

        /// <summary>
        /// Вывод дерева в консоль
        /// </summary>
        /// <param name="tree"></param>
        private void PrintTreeConsole(TreeNode node, int level)
        {
            if (node == null)
                return;

            Console.WriteLine(new string(' ', 3 * level) + "|_" + node.Value);

            foreach (var child in node.Children)
            {
                PrintTreeConsole(child, level + 1);
            }
        }

        /// <summary>
        /// Вывод в файл кода
        /// </summary>
        /// <param name="codeAnswer"></param>
        private void WriteToFileCode(string codeAnswer)
        {
            string path = @"files/preuferCodeAnswer.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(codeAnswer);
            }
        }

        /// <summary>
        /// Вывод в файл дерева
        /// </summary>
        /// <param name="listAnswer"></param>
        private void WriteToFileAndConsoleTree(List<int[]> ribsList)
        {
            string path = @"files/codePreuferInTreeAnswer.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                Console.WriteLine("Ребра дерева:");
                writer.WriteLine("Ребра дерева:");
                foreach (int[] rib in ribsList)
                {
                    Console.WriteLine(string.Join(" ", rib));
                    writer.WriteLine(string.Join(" ", rib));
                }
            }
        }
    }
}