using System;
using System.Diagnostics;

namespace Bai3_TuCaiDatHangDoiVong
{
    public class CircularQueue<T>
    {
        private T[] items;      // Mảng tĩnh lưu trữ phần tử nội bộ
        private int front;      // Chỉ số của phần tử đứng ở đầu hàng đợi
        private int rear;       // Chỉ số vị trí trống tiếp theo ở cuối hàng đợi
        private int count;      // Số lượng phần tử thực tế hiện có trong hàng đợi

        // Thuộc tính Count lấy số phần tử hiện tại (chỉ đọc)
        public int Count
        {
            get { return count; }
        }

        // Thuộc tính Capacity lấy dung lượng mảng đệm hiện tại (chỉ đọc)
        public int Capacity
        {
            get { return items.Length; }
        }

        // Khởi tạo hàng đợi vòng với dung lượng ban đầu là 4 theo đúng quy định đề bài
        public CircularQueue()
        {
            items = new T[4];   // Cấp phát mảng ban đầu 4 ô nhớ
            front = 0;          // Chỉ số đầu hàng ban đầu là 0
            rear = 0;           // Chỉ số cuối hàng ban đầu là 0
            count = 0;          // Số phần tử ban đầu là 0
        }

        // Thêm phần tử vào cuối hàng đợi; tự mở rộng mảng gấp đôi nếu bị đầy
        public void Enqueue(T value)
        {
            // Nếu hàng đợi đã đầy (số phần tử bằng dung lượng mảng)
            if (count == items.Length)
            {
                // Mở rộng mảng gấp đôi dung lượng hiện tại
                Resize(items.Length * 2);
            }

            // Gán phần tử mới vào ô trống tại chỉ số rear
            items[rear] = value;

            // Tính chỉ số kế tiếp của rear theo công thức quay vòng mảng
            rear = (rear + 1) % items.Length;

            // Tăng số phần tử hiện có lên 1
            count++;
        }

        // Lấy và xóa phần tử ở đầu hàng đợi
        public T Dequeue()
        {
            // Ném ngoại lệ InvalidOperationException nếu hàng đợi rỗng
            if (count == 0)
            {
                throw new InvalidOperationException("Queue empty, can't dequeue");
            }

            // Lấy giá trị của phần tử đứng ở đầu hàng tại chỉ số front
            T result = items[front];

            // Đặt ô vừa lấy về giá trị mặc định default để bộ thu gom rác (GC) giải phóng bộ nhớ
            items[front] = default!;

            // Tăng chỉ số front theo công thức quay vòng mảng, không dồn mảng
            front = (front + 1) % items.Length;

            // Giảm số phần tử hiện có đi 1
            count--;

            // Trả về giá trị phần tử vừa lấy ra
            return result;
        }

        // Xem phần tử ở đầu hàng đợi nhưng không xóa
        public T Peek()
        {
            // Ném ngoại lệ InvalidOperationException nếu hàng đợi rỗng
            if (count == 0)
            {
                throw new InvalidOperationException("Queue empty, can't Peek");
            }

            // Trả về giá trị tại chỉ số front mà không thay đổi trạng thái hàng đợi
            return items[front];
        }

        // Mở rộng mảng gấp đôi và sao chép theo đúng thứ tự logic từ front bằng Array.Copy
        private void Resize(int newCapacity)
        {
            T[] newArray = new T[newCapacity];

            if (front < rear)
            {
                // Trường hợp 1: Dữ liệu nằm liên tục từ front đến rear (chưa bị xoay vòng)
                Array.Copy(items, front, newArray, 0, count);
            }
            else
            {
                // Trường hợp 2: Dữ liệu bị xoay vòng (đoạn 1 từ front đến cuối mảng, đoạn 2 từ 0 đến rear)
                int rightSegmentSize = items.Length - front; // Số phần tử ở đoạn bên phải

                // Chép đoạn bên phải từ front đến cuối mảng cũ vào đầu mảng mới
                Array.Copy(items, front, newArray, 0, rightSegmentSize);

                // Chép đoạn bên trái từ 0 đến rear nối tiếp vào mảng mới
                Array.Copy(items, 0, newArray, rightSegmentSize, rear);
            }

            // Cập nhật lại mảng dữ liệu mới
            items = newArray;

            // Đặt lại con trỏ front về vị trí 0 đầu mảng mới
            front = 0;

            // Đặt con trỏ rear về vị trí count (chỉ số trống ngay sau phần tử cuối)
            rear = count;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // STT 1: Hàng rỗng -> Dequeue và Peek đều ném ngoại lệ InvalidOperationException
            Console.WriteLine("Case 1");
            var q1 = new CircularQueue<string>();
            Console.WriteLine($"Count: {q1.Count}, Capacity: {q1.Capacity}");

            try { q1.Dequeue(); }
            catch (InvalidOperationException ex) { Console.WriteLine($"[Bat loi Dequeue]: {ex.Message}"); }

            try { q1.Peek(); }
            catch (InvalidOperationException ex) { Console.WriteLine($"[Bat loi Peek]: {ex.Message}"); }

            // STT 2: Enqueue A, B, C, D; Peek
            Console.WriteLine("Case 2");
            var q2 = new CircularQueue<string>();
            q2.Enqueue("A"); q2.Enqueue("B"); q2.Enqueue("C"); q2.Enqueue("D");
            Console.WriteLine($"Count: {q2.Count}, Capacity: {q2.Capacity}, Peek: {q2.Peek()}");

            // STT 3: Enqueue A, B, C, D; Dequeue 2 lần; Enqueue E, F; Dequeue 4 lần
            Console.WriteLine("Case 3");
            var q3 = new CircularQueue<string>();
            q3.Enqueue("A"); q3.Enqueue("B"); q3.Enqueue("C"); q3.Enqueue("D");
            Console.WriteLine($" {q3.Dequeue()}, {q3.Dequeue()}"); 
            q3.Enqueue("E"); q3.Enqueue("F");                                        
            Console.WriteLine($"Capacity: {q3.Capacity}");
            Console.Write("Lay ra 4 lan: ");
            while (q3.Count > 0) Console.Write(q3.Dequeue() + " "); 
            Console.WriteLine();

            // STT 4: Enqueue 1, 2, 3, 4, 5; Dequeue 5 lần
            Console.WriteLine("Case 4");
            var q4 = new CircularQueue<int>();
            for (int i = 1; i <= 5; i++) q4.Enqueue(i);
            Console.WriteLine($"Capacity sau khi mo rong: {q4.Capacity}"); // Capacity 8
            Console.Write("Lay ra 5 lan: ");
            while (q4.Count > 0) Console.Write(q4.Dequeue() + " "); // Kết quả: 1 2 3 4 5
            Console.WriteLine();

            // STT 5: Enqueue A, B, C, D; Dequeue 2 lần; Enqueue E, F, G; Dequeue 5 lần
            Console.WriteLine("Case 5");
            var q5 = new CircularQueue<string>();
            q5.Enqueue("A"); q5.Enqueue("B"); q5.Enqueue("C"); q5.Enqueue("D");
            q5.Dequeue(); q5.Dequeue(); // Lấy A, B -> front = 2
            q5.Enqueue("E"); q5.Enqueue("F"); q5.Enqueue("G"); // Đầy và Resize khi front = 2
            Console.WriteLine($"New Capacity: {q5.Capacity}"); // Capacity 8
            Console.Write("Lay ra 5 lan: ");
            while (q5.Count > 0) Console.Write(q5.Dequeue() + " "); // Kết quả: C D E F G
            Console.WriteLine();

            // STT 6: Lặp 1 000 000 lần: Enqueue rồi Dequeue
            Console.WriteLine("Case 6");
            var q6 = new CircularQueue<int>();
            for (int i = 0; i < 1000000; i++)
            {
                q6.Enqueue(i);
                q6.Dequeue();
            }
            Console.WriteLine($"Success! Count: {q6.Count}, Capacity: {q6.Capacity}"); // Capacity vẫn 4

            // STT 7: Enqueue 100 000 phần tử rồi Dequeue hết
            Console.WriteLine("Case 7");
            var q7 = new CircularQueue<int>();
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 1; i <= 100000; i++) q7.Enqueue(i);
            int finalCapacity = q7.Capacity;
            bool isCorrect = true;
            for (int i = 1; i <= 100000; i++)
            {
                if (q7.Dequeue() != i) isCorrect = false;
            }
            sw.Stop();
            Console.WriteLine($"Lay ra dung thu tu: {isCorrect}");
            Console.WriteLine($"Capacity cuoicung: {finalCapacity}");
            Console.WriteLine($"Thoi gian chay: {sw.ElapsedMilliseconds} ms");

        }
    }
}