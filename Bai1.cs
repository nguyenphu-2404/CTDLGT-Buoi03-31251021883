using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bai1_KiemTraDauNgoac
{
    internal class Program
    {
        public static string CheckBalancedParentheses(string input)
        {
            // Chuỗi rỗng hoặc null được tính là hợp lệ
            if (string.IsNullOrEmpty(input))
            {
                return "HOP LE";
            }

            // Stack lưu tuple (char, int) chứa ký tự ngoặc mở và vị trí index gốc của nó trong chuỗi
            // Lưu vị trí giúp xác định chính xác chỉ số ngoặc bị lỗi mà không cần duyệt lại chuỗi từ đầu
            var st = new Stack<(char ch, int pos)>();

            // Duyệt từng ký tự từ đầu đến cuối chuỗi theo chỉ số i tăng dần
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // Nếu là ngoặc mở, Push ký tự cùng vị trí i vào Stack để chờ ngoặc đóng tương ứng
                if (c == '(' || c == '[' || c == '{')
                {
                    st.Push((c, i));
                }
                // Nếu ký tự là một trong ba loại ngoặc đóng
                else if (c == ')' || c == ']' || c == '}')
                {
                    // Lỗi ngoặc đóng thừa: Gặp ngoặc đóng nhưng Stack rỗng (không có ngoặc mở nào chờ đóng)
                    // Dừng thuật toán và báo ngay vị trí i của ngoặc đóng thừa này
                    if (st.Count == 0)
                    {
                        return $"SAI tai vi tri {i}";
                    }

                    // Lấy ngoặc mở mới nhất trên đỉnh Stack ra để kiểm tra tính khớp cặp
                    var top = st.Pop();

                    // Lỗi đóng sai loại: Ngoặc đóng c không cùng loại với ngoặc mở top.ch vừa lấy
                    // Dừng thuật toán và báo vị trí i của ngoặc đóng vi phạm
                    if (!MatchPair(top.ch, c))
                    {
                        return $"SAI tai vi tri {i}";
                    }
                }
            }

            // Sau khi duyệt hết chuỗi, kiểm tra nếu Stack vẫn còn ngoặc mở chưa được Pop ra
            if (st.Count > 0)
            {
                // Lỗi ngoặc mở chưa đóng: Các ngoặc mở trong Stack không có ngoặc đóng tương ứng
                // Yêu cầu: Báo vị trí ngoặc mở chưa đóng xuất hiện sớm nhất trong chuỗi
                // ToArray() trả về mảng có phần tử [0] là đỉnh và phần tử cuối [Length - 1] là đáy Stack (ngoặc mở sớm nhất)
                var stackArray = st.ToArray();
                var Unclosed = stackArray[stackArray.Length - 1];

                // Báo vị trí của ngoặc mở chưa đóng xuất hiện sớm nhất
                return $"SAI tai vi tri {Unclosed.pos}";
            }

            // Chuỗi hợp lệ: Tất cả ngoặc mở đều được đóng đúng cặp và đúng thứ tự
            return "HOP LE";
        }

        // Hàm hỗ trợ kiểm tra xem cặp ngoặc mở và ngoặc đóng có cùng loại hay không
        private static bool MatchPair(char open, char close)
        {
            // Trả về true nếu thuộc đúng một trong ba cặp (), [], {}
            return (open == '(' && close == ')') ||
                   (open == '[' && close == ']') ||
                   (open == '{' && close == '}');
        }
        static void Main1(string[] args)
        {
            // 1. Kiểm tra 1 chuỗi nhập thủ công từ bàn phím
            /*Console.Write("Input: ");
            string input = Console.ReadLine();
            string result = CheckBalancedParentheses(input);
            Console.WriteLine($"Output: {result}\n");*/

            // 2. Case 9: 500,000 ngoặc '(' + 500,000 ngoặc ')'
            string test9 = new string('(', 500000) + new string(')', 500000);
            Stopwatch sw = Stopwatch.StartNew();
            string res9 = CheckBalancedParentheses(test9);
            sw.Stop();
            Console.WriteLine($"Ca 9 -> Output: {res9} | Thoi gian: {sw.ElapsedMilliseconds} ms");

            // 3. Case 10: 1,000,000 ngoặc '('
            string test10 = new string('(', 1000000);
            sw.Restart();
            string res10 = CheckBalancedParentheses(test10);
            sw.Stop();
            Console.WriteLine($"Ca 10 -> Output: {res10} | Thoi gian: {sw.ElapsedMilliseconds} ms");
        }
    }
}
