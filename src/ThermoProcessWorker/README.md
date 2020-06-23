
# processWorker


            var fileJson = File.ReadAllText(@"c:\dev\test2.json");
            var x = JsonConvert.DeserializeObject<AttendanceResponse>(fileJson);
            Console.WriteLine("Hello World!");
