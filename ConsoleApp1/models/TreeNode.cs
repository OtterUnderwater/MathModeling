namespace ConsoleApp1.models
{
    /// <summary>
    /// Дерево для кода Прюфера и других
    /// </summary>
    public class TreeNode
    {
        public int Value { get; set; }    
        public List<TreeNode> Children { get; set;}
        public TreeNode? Parent { get; set; }

        public TreeNode(int value)
        {
            Value = value;
            Children = new List<TreeNode>();
            Parent = null;
        }
        public void AddChildren(TreeNode child)
        {
            Children.Add(child);
            child.Parent = this;
        }
    }
}


