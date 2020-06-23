# Thermo.Service.Worker

    var fileJson = File.ReadAllText(@"c:\dev\test.json");
            var x = JsonConvert.DeserializeObject<AttendanceResponse>(fileJson);
            Console.WriteLine("Hello World!");

  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="12.0.3" />
  </ItemGroup>
