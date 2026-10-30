using System;

namespace Bai4_MinStack
{
    public class MinStack
    {
        // Sử dụng một Stack lưu (val, currentMin)
        // val: giá trị thực sự được Push vào
        // currentMin: giá trị nhỏ nhất trong Stack tính tới thời điểm phần tử đó được Push
        private readonly System.Collections.Generic.Stack<(int val, int currentMin)> st;

        // Lấy số lượng phần tử hiện có trong Stack
        public int Count => st.Count;

        public MinStack()
        {
            st = new System.Collections.Generic.Stack<(int val, int currentMin)>();
        }

        // Thêm phần tử x vào Stack 
        public void Push(int x)
        {
            // Nếu Stack rỗng, giá trị nhỏ nhất chính là x
            // Nếu Stack đã có phần tử, min mới = Math.Min(x, min_ở_đỉnh_Stack)
            int newMin = (st.Count == 0) ? x : Math.Min(x, st.Peek().currentMin);

            // Push cả giá trị x và newMin vào Stack
            st.Push((x, newMin));
        }

        // Lấy và xóa phần tử ở đỉnh Stack - Độ phức tạp O(1)
        public int Pop()
        {
            // Nếu Stack rỗng, ném InvalidOperationException theo yêu cầu đề bài
            if (st.Count == 0)
            {
                throw new InvalidOperationException("Stack's empty, can't use Pop");
            }

            // Pop tuple ở đỉnh và trả về giá trị val
            return st.Pop().val;
        }

        // Lấy giá trị nhỏ nhất hiện tại trong Stack - Độ phức tạp O(1)
        public int GetMin()
        {
            // Nếu Stack rỗng, ném InvalidOperationException theo yêu cầu đề bài
            if (st.Count == 0)
            {
                throw new InvalidOperationException("Stack's empty, can't use GetMin");
            }

            // Giá trị nhỏ nhất luôn nằm ở thành phần currentMin của phần tử đỉnh Stack
            return st.Peek().currentMin;
        }
    }

    internal class Program
    {
        static void Main4(string[] args)
        {
            var minStack = new MinStack();
            int[] pushValues = { 5, 3, 7, 3, 8 };
            foreach (var v in pushValues)
            {
                minStack.Push(v);
                Console.WriteLine($"Push({v}) -> GetMin(): {minStack.GetMin()}");
            }

            // Pop 4 lần liên tiếp và kiểm tra GetMin sau mỗi lần
            for (int i = 1; i <= 4; i++)
            {
                int popped = minStack.Pop();
                Console.WriteLine($"Pop() ra {popped} -> GetMin() con lai: {minStack.GetMin()}");
            }

            // Kiểm tra trường hợp biên: Pop/GetMin trên Stack rỗng
            minStack.Pop(); 
            try
            {
                minStack.GetMin();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[Bat loi GetMin Thanh Cong]: {ex.Message}");
            }
        }
    }
}