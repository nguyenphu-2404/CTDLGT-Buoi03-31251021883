using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Bai2_MoPhongQuayDichVu
{
    // Record lưu trữ thông tin cố định của từng khách hàng:
    public record Customer(int Id, long ArrivalTime, long ServiceTime);

    internal class Program
    {
        public static void SimulateService(int n, int k, List<(long a, long s)> inputCustomers)
        {
            var queue = new Queue<Customer>();

            // Duyệt danh sách đầu vào để tạo các đối tượng Customer và nạp lần lượt vào Queue
            for (int i = 0; i < n; i++)
            {
                // Gán Id từ 1 đến n tương ứng với vị trí xếp hàng ban đầu của khách
                queue.Enqueue(new Customer(i + 1, inputCustomers[i].a, inputCustomers[i].s));
            }

            // Mảng lưu mốc thời gian rảnh rỗi tiếp theo của k quầy (chỉ số 0 tương ứng quầy 1, chỉ số k-1 tương ứng quầy k)
            // Khởi tạo mặc định tất cả các quầy đều rảnh tại mốc thời gian 0
            long[] counterFreeTime = new long[k];

            // Biến long lưu tổng thời gian chờ của tất cả khách hàng 
            long totalWaitTime = 0;

            // Biến long ghi nhận thời gian chờ lâu nhất của một khách bất kỳ
            long maxWaitTime = 0;

            // Biến long ghi nhận mốc thời gian hoàn thành muộn nhất của toàn hệ thống
            long maxEndTime = 0;

            // Vòng lặp xử lý lần lượt từng khách hàng cho đến khi Hàng đợi rỗng
            while (queue.Count > 0)
            {
                // Lấy khách hàng ở đầu Hàng đợi ra để phục vụ theo đúng nguyên tắc FIFO
                Customer cust = queue.Dequeue();

                // Giả định quầy thứ 0 (quầy 1) là quầy rảnh sớm nhất ban đầu
                int bestCounter = 0;
                long minFreeTime = counterFreeTime[0];

                // Quét qua k - 1 quầy còn lại để tìm quầy có thời điểm rảnh nhỏ nhất
                // Dùng dấu '<' để nếu nhiều quầy rảnh cùng lúc thì chọn quầy có chỉ số nhỏ nhất
                for (int q = 1; q < k; q++)
                {
                    if (counterFreeTime[q] < minFreeTime)
                    {
                        minFreeTime = counterFreeTime[q];
                        bestCounter = q;
                    }
                }

                // Thời điểm bắt đầu phục vụ = MAX(Thời điểm khách đến, Thời điểm quầy rảnh)
                long startTime = Math.Max(cust.ArrivalTime, counterFreeTime[bestCounter]);

                // Thời gian chờ của khách = Thời điểm bắt đầu phục vụ - Thời điểm khách đến
                long waitTime = startTime - cust.ArrivalTime;

                // Thời điểm hoàn thành dịch vụ = Thời điểm bắt đầu phục vụ + Thời gian phục vụ
                long endTime = startTime + cust.ServiceTime;

                // Cập nhật mốc thời gian rảnh mới cho quầy vừa được chọn
                counterFreeTime[bestCounter] = endTime;

                // Cộng dồn thời gian chờ của khách hiện tại vào tổng thời gian chờ
                totalWaitTime += waitTime;

                // Cập nhật thời gian chờ lâu nhất nếu thời gian chờ của khách này lớn hơn
                if (waitTime > maxWaitTime)
                {
                    maxWaitTime = waitTime;
                }

                // Cập nhật mốc thời gian hoàn thành muộn nhất của toàn bộ chương trình
                if (endTime > maxEndTime)
                {
                    maxEndTime = endTime;
                }

                // In chi tiết từng khách hàng (chỉ áp dụng với n nhỏ để tránh làm chậm cửa sổ console)
                if (n <= 20)
                {
                    Console.WriteLine($"KH {cust.Id}: den {cust.ArrivalTime}, quay {bestCounter + 1}, bat dau {startTime}, cho {waitTime}, xong {endTime}");
                }
            }

            // Tính thời gian chờ trung bình 
            double avgWaitTime = (double)totalWaitTime / n;

            // In tổng hợp kết quả theo định dạng số thực 2 chữ số thập phân dùng CultureInfo.InvariantCulture
            Console.WriteLine($"Cho trung binh: {avgWaitTime.ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Cho lau nhat: {maxWaitTime}");
            Console.WriteLine($"Hoan thanh luc: {maxEndTime}");
        }

        static void Main2(string[] args)
        {
            /*// Ca 1: k = 1, 3 khách
            Console.WriteLine("Ca 1 (k = 1) ");
            SimulateService(3, 1, new List<(long, long)> { (0, 5), (1, 3), (2, 4) });

            // Ca 2: k = 2, 3 khách
            Console.WriteLine(" Ca 2 (k = 2) ");
            SimulateService(3, 2, new List<(long, long)> { (0, 5), (1, 3), (2, 4) });

            // Ca 3: k = 2, 3 
            Console.WriteLine(" Ca 3 (Den cung luc) ");
            SimulateService(3, 2, new List<(long, long)> { (0, 3), (0, 3), (0, 3) });

            // Ca 4: k = 1, quầy rảnh rồi mới có khách
            Console.WriteLine(" Ca 4 (Quay ranh cho khach) ");
            SimulateService(2, 1, new List<(long, long)> { (0, 2), (10, 3) });

            // Ca 5: n = 1, k = 3
            Console.WriteLine(" Ca 5 (n = 1, k = 3) ");
            SimulateService(1, 3, new List<(long, long)> { (4, 6) });*/

           // Ca 6: Ca lớn n = 100,000 khách, k = 10 quầy
            Console.WriteLine(" Ca 6 (Ca lon: n = 100,000, k = 10) ");
            int largeN = 100000;
            int largeK = 10;
            var largeList = new List<(long, long)>(largeN);
            for (int i = 0; i < largeN; i++)
            {
                largeList.Add((i, 10)); 
            }

            Stopwatch sw = Stopwatch.StartNew();
            SimulateService(largeN, largeK, largeList);
            sw.Stop();
            Console.WriteLine($"Thoi gian Ca 6: {sw.ElapsedMilliseconds} ms"); 
        }
    }
}
