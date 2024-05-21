using ConsoleApp1.models;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;

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
            foreach (int[] code in codeList)
            {
                int parent = code[0];
                int child = code[1];
                if (!tree.ContainsKey(parent))
                {
                    tree[parent] = new TreeNode(parent);
                }
                if (!tree.ContainsKey(child))
                {
                    tree[child] = new TreeNode(child);
                }
                tree[parent].AddChildren(tree[child]);
            }
            Console.WriteLine("Дерево:");
            PrintTree(tree);
            while (tree.Count > 2)
            {
                // Получаем все листы
                List<TreeNode> leaf = tree.Values.Where(node => node.Children.Count == 0 || (node.Children.Count == 1 && node.Parent == null)).ToList();
                TreeNode minLeaf = leaf.OrderBy(x => x.Value).First(); // находим минимальный лист
                // добавляем связанный элемент в код и удаляем лист
                if (minLeaf.Parent == null) // Проверяем, является ли minLeaf корневым узлом
                {
                    codeAnswer += minLeaf.Children[0].Value;
                    tree.Remove(minLeaf.Value);
                }
                else
                {
                    codeAnswer += minLeaf.Parent.Value;
                    TreeNode parent = tree[minLeaf.Parent.Value]; // нашли родителя
                    parent.Children.Remove(minLeaf);  // удаление minLeaf из списка потомков
                    tree.Remove(minLeaf.Value);
                }
            }
            Console.WriteLine("Код прюфера: " + codeAnswer);
            WriteToFile("Код прюфера: " + codeAnswer);
        }

        /// <summary>
        /// Вывод дерева
        /// </summary>
        /// <param name="tree"></param>
        public void PrintTree(Dictionary<int, TreeNode> tree) => PrintNode(tree[1], 0);

        /// <summary>
        /// Вывод одного корня
        /// </summary>
        /// <param name="node"></param>
        /// <param name="level"></param>
        private void PrintNode(TreeNode node, int level)
        {
            if (node == null)
                return;

            Console.WriteLine(new string(' ', 3 * level) + "|_" + node.Value);

            foreach (var child in node.Children)
            {
                PrintNode(child, level + 1);
            }
        }

        /// <summary>
        /// Вывод в файл
        /// </summary>
        /// <param name="codeAnswer"></param>
        private void WriteToFile(string codeAnswer)
        {
            string path = @"files/preuferCodeAnswer.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(codeAnswer);
            }
        }
    }
}