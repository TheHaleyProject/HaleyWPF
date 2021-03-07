using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Haley.Log;
using Haley.Abstractions;
using Haley.Models;
using Haley.Events;
using System.Threading;
using Haley.MVVM;
using Haley.RuleEngine;
using Haley.Enums;
using Haley.Utils;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace DevelopmentConsole
{
    #region Image Processing
    //public class Program
    //{
    //    #region ATTRIBUTES

    //    #endregion
    //    private static string file_path;
    //    private static string file_name;
    //    private static string saved_path;
    //    private static string new_file_path;
    //    private static int pixel_width;
    //    private static int pixel_height;
    //    private static PixelFormat _format;
    //    private static int pixel_width_cropped;
    //    private static int pixel_height_cropped;

    //    public static void Main(string[] args)
    //    {
    //        //PREPARE VARIABLES

    //        //string file_path = $@"D:\Study Material\Machine Learning\Input Processing\Input\TestSet\x_test_gr_smpl.csv";
    //        file_path = $@"D:\Study Material\Machine Learning\Input Processing\Input\Stage 02\x_train_gr_smpl_cropped_6px.csv";
    //        file_name = Path.GetFileNameWithoutExtension(file_path);
    //        saved_path = Path.Combine(Path.GetDirectoryName(file_path), "Outputs");
    //        new_file_path = Path.Combine(saved_path, file_name);

    //        if (!Directory.Exists(saved_path)) Directory.CreateDirectory(saved_path);

    //        _format = PixelFormats.Gray8; //For monochrome.

    //        bool _has_header = true;
    //        bool _has_id = false;

    //        pixel_width = 36;
    //        pixel_height = 36;

    //        int topbottom_crop = 0;
    //        int leftright_crop = 0;

    //        pixel_width_cropped = pixel_width - (leftright_crop * 2);
    //        pixel_height_cropped = pixel_height - (topbottom_crop * 2);

    //        string _de_limiter = ",";

    //        //FETCH THE DATA
    //        var _processed_data = process(file_path, _de_limiter, 0);

    //        int instance_to_check = 125; //Any value from user
    //        if (instance_to_check > _processed_data.Count) return; //Items to check should be with in the limit.

    //        ////CROP THE DATA
    //        //var to_crop = get_indexs_to_crop(_processed_data[0].Count, pixel_width, pixel_height, _format, topbottom_crop, leftright_crop);

    //        //List<List<string>> _cropped_data = new List<List<string>>();
    //        //foreach (var _instance in _processed_data) // for each list of data, get new list
    //        //{
    //        //    List<string> cropped_image_data = new List<string>();
    //        //    for (int i = 0; i < _instance.Count; i++)
    //        //    {
    //        //        if (to_crop.indexs_to_remove.Contains(i)) continue; //This index has to be ignored.
    //        //        cropped_image_data.Add(_instance[i]);// Add this string
    //        //    }
    //        //    _cropped_data.Add(cropped_image_data);
    //        //}
    //        //write_csv(_cropped_data, $@"{new_file_path}_cropped.csv");

    //        //NORMALIZE THE DATA
    //        int normalize_counter = 0;
    //        List<List<string>> _normalized_data = new List<List<string>>();
    //        foreach (var _instance in _processed_data) // for each instance
    //        {

    //            List<string> _normalized_instance = new List<string>();

    //            if (_has_header && normalize_counter == 0) //If we have headers, just return same value
    //            {
    //                _normalized_instance = _instance;
    //            }
    //            else
    //            {
    //                List<double> _instance_double_cast = _instance.Select(p => double.Parse(p)).ToList();

    //                double max_value = _instance_double_cast.Max();
    //                double min_value = _instance_double_cast.Min();

    //                foreach (var _field in _instance_double_cast)
    //                {
    //                    string _normalized_field = Math.Round((_field - min_value) / (max_value - min_value), 4).ToString();
    //                    _normalized_instance.Add(_normalized_field);
    //                }
    //            }

    //            _normalized_data.Add(_normalized_instance);

    //            normalize_counter++; //move to next instance
    //        }

    //        write_csv(_normalized_data, $@"{new_file_path}_normalized.csv");


    //        #region CHECKING
    //        //CONVERT TO 2D ARRAY FOR DISPLAYING
    //        //var converted = convert_to_2D_array(_processed_data[instance_to_check], pixel_width, pixel_height, _format);
    //        //check_images(instance_to_check, _has_id, _processed_data, _cropped_data);

    //        #endregion
    //    }

    //    public static void check_images(int instance_to_check, bool _has_id, List<List<string>> _processed_data, List<List<string>> _cropped_data = null)
    //    {
    //        try
    //        {
    //            //CHECK ORIGINAL IMAGE
    //            var pixel_data = getImage(_processed_data[instance_to_check], _has_id); //This tries to convert and get the instance data.
    //            int expected_pixels = pixel_width * pixel_height;
    //            if (expected_pixels != pixel_data.image_array.Length) return; //Because pixel values are wrong.
    //                                                                          //Now use the image data and visualize
    //            ImageSource img = ImageAPI.pixelsToImageSource(pixel_width, pixel_height, pixel_data.image_array, _format, 20, 20);
    //            ImageAPI.saveImageSource(img, $@"{saved_path}\{instance_to_check}.png");

    //            //CHECK CROPPED IMAGE
    //            if (_cropped_data != null)
    //            {
    //                var pixel_data_cropped = getImage(_cropped_data[instance_to_check], _has_id); //This tries to convert and get the instance data.
    //                int expected_pixels_cropped = pixel_width_cropped * pixel_height_cropped;
    //                if (expected_pixels_cropped != pixel_data_cropped.image_array.Length) return; //Because pixel values are wrong.
    //                                                                                              //Now use the image data and visualize
    //                ImageSource img_cropped = ImageAPI.pixelsToImageSource(pixel_width_cropped, pixel_height_cropped, pixel_data_cropped.image_array, _format, 20, 20);
    //                ImageAPI.saveImageSource(img_cropped, $@"{saved_path}\{instance_to_check}_cropped.png");
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public static List<List<string>> process(string file_path, string _de_limiter = ";", int conversion_limit = 0) //Update it later to do with yield return. Also, it would be better to do a task or on a different thread.
    //    {
    //        List<List<string>> row_data = new List<List<string>>(); //Parse all data as string. then convert as required.
    //        int row_count = 0;
    //        int column_count = 0;
    //        using (StreamReader sreader = new StreamReader(file_path))
    //        {
    //            using (CsvReader _reader = new CsvReader(sreader))
    //            {
    //                _reader.Configuration.Delimiter = _de_limiter;

    //                while (_reader.Read()) //Read each line
    //                {
    //                    //CHECK CONVERSION LIMITS. If there is a conversion limit, then convert only up to that instance. 
    //                    if (conversion_limit != 0)
    //                    {
    //                        if (row_count > conversion_limit) break;
    //                    }

    //                    //INITIATE ROW LEVEL VARIABLES
    //                    column_count = 0;
    //                    bool has_data = true;
    //                    List<string> column_values = new List<string>(); //We don't know the count of columns available yet.

    //                    //READ A ROW
    //                    while (has_data) //ASSUMING THAT THERE IS NO MISSING VALUES
    //                    {
    //                        string field_data;

    //                        has_data = _reader.TryGetField<string>(column_count, out field_data); //Try to get the value as a string.
    //                        if (has_data == false) break; //Don't continue further
    //                        column_values.Add(field_data);
    //                        column_count++;
    //                    }
    //                    row_data.Add(column_values);
    //                    row_count++;
    //                }; //Read the data.
    //            }
    //        }
    //        return row_data;
    //    }

    //    public static (byte[] image_array, bool success) getImage(List<string> instance_value, bool has_id = false)
    //    {
    //        try
    //        {
    //            bool _success = true;
    //            byte[] image_data = new byte[instance_value.Count]; // To match the list
    //            //Input is string. So, try to convert it to double and check if the value is between 0 and 255. If error, return the whole data out.
    //            //If id column is present then ignore last column
    //            int columns = instance_value.Count;
    //            if (has_id) columns -= 1; //Ignore one column from last
    //            for (int i = 0; i < columns; i++)
    //            {
    //                string _field = instance_value[i];
    //                double _converted_field = 0;

    //                //CHECK IF IT IS A DOUBLE VALUE
    //                _success = double.TryParse(_field, out _converted_field);
    //                if (!_success) return (image_data, _success);//If unable to parse in to double

    //                //CHECK IF IT IS WITH IN COLOR VALUE RANGE
    //                if (_converted_field < 0 || _converted_field > 255) return (image_data, _success); //Value is not a color value

    //                //If previous steps were succesful, then try to convert it to byte
    //                byte _field_byte = byte.Parse(_converted_field.ToString()); //
    //                image_data[i] = _field_byte;
    //            }

    //            return (image_data, _success);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static (List<int> indexs_to_remove, bool success) get_indexs_to_crop(int data_count, int pixel_width, int pixel_height, PixelFormat _format, int top_bottom_crop, int left_right_crop, bool has_id = false)
    //    {
    //        try
    //        {
    //            List<int> _index_to_remove = new List<int>();
    //            bool _is_success = true;

    //            //based on pixel format calculate the stride.
    //            int bytes_per_pixel = (_format.BitsPerPixel + 7) / 8; //it gives rounded byte value.
    //            int stride = pixel_width * bytes_per_pixel; //No of data per row

    //            if (has_id) data_count -= 1; //Remove last value

    //            //CHECK IF THE PIXELS MATCH 
    //            if (!(stride * pixel_height == data_count)) return (_index_to_remove, false);

    //            //GET THE RANGE
    //            int vertical_items = stride * top_bottom_crop; //This gives items to remove in vertical list
    //            int horizontal_items = bytes_per_pixel * left_right_crop; //This gives items to remove in horizontal array

    //            for (int i = 0; i < data_count; i++) //loop through each value
    //            {
    //                //CHECK TOP / BOTTOM RANGE
    //                if (i > vertical_items && i < data_count - vertical_items)
    //                {
    //                    int current_item = i;
    //                    if (current_item > stride) current_item = (current_item % stride); //This sets the value with in the stride range, by getting the remainder.

    //                    //CHECK LEFT/RIGHT RANGE
    //                    if (current_item < horizontal_items + 1 || current_item > stride - horizontal_items)
    //                    {
    //                        _index_to_remove.Add(i); //this item falls inside the left / right removal range
    //                    }
    //                }
    //                else //this item falls inside top/bottom removal range
    //                {
    //                    _index_to_remove.Add(i);
    //                }
    //            }

    //            return (_index_to_remove, _is_success);
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }

    //    public static (List<List<string>> pixel_array, bool success) convert_to_2D_array(List<string> instance_value, int pixel_width, int pixel_height, PixelFormat _format, bool has_id = false) //Purely for displaying to user for validation
    //    {
    //        try
    //        {
    //            //based on pixel format calculate the stride.
    //            int bytes_per_pixel = (_format.BitsPerPixel + 7) / 8; //it gives rounded byte value.
    //            int stride = pixel_width * bytes_per_pixel; //No of data per row

    //            List<List<string>> _pixel_array = new List<List<string>>();
    //            bool one_pixel_row_done = true;
    //            int i = 0;

    //            int field_count = instance_value.Count;
    //            if (has_id) field_count -= 1;//Remove last attribute

    //            while (i < field_count)
    //            {
    //                List<string> _pixel_column = new List<string>();
    //                for (int j = 0; j < stride; j++) //process for one row of pixel data
    //                {
    //                    _pixel_column.Add(instance_value[i]); //add the value from unorganized list to organized column list
    //                    i++; //increment i.
    //                }
    //                _pixel_array.Add(_pixel_column);
    //            }

    //            int result_count = _pixel_array.Sum(p => p.Count());

    //            return (_pixel_array, field_count == result_count);
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public static void write_csv(List<List<string>> records, string file_save_path)
    //    {
    //        try
    //        {
    //            using (var stream = new MemoryStream())
    //            using (var writer = new StreamWriter(stream))
    //            using (var csv = new CsvWriter(writer))
    //            {
    //                foreach (var record in records)
    //                {
    //                    foreach (var field in record)
    //                    {
    //                        csv.WriteField(field);
    //                    }
    //                    csv.NextRecord();
    //                }
    //                //Write stream to file
    //                FileStream new_file = new FileStream(file_save_path, FileMode.Create, FileAccess.Write);
    //                writer.Flush();
    //                stream.Position = 0;
    //                stream.CopyTo(new_file);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }

    //}
    #endregion

    #region Log Processing
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        //string _path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    //        //HLog _log = new HLog(Path.Combine(_path, "Hello"), "globalLog", OutputType.txt_detailed, max_memory_count: 100);

    //        HLog _log = new HLog(OutputType.txt_detailed);

    //        LogStore.CreateSingleton(_log);

    //        var _baselog = LogStore.Singleton.BaseLog;

    //        _baselog.log("Software Initiated", property_name: "Main Module", in_memory: true);
    //        _baselog.debug("What the hell");
    //        for (int i = 1; i < 10; i++)
    //        {
    //            _baselog.log($@"Newlog entry {i}", in_memory: true);
    //        }

    //        for (int i = 1; i < 5; i++)
    //        {
    //            _baselog.log($@"This is a sub items for testing {i}", in_memory: true, is_sub: true);
    //        }
    //        _baselog.dumpMemory();
    //        _baselog.log($@"Beginning of new data");

    //        for (int i = 0; i < 5; i++)
    //        {
    //            _baselog.log($@"This is a sub items for testing {i}", in_memory: true, is_sub: true);
    //        }

    //        _baselog.log("New entry to check if memory is dumped");
    //        _baselog.log("", property_name: "user Details");
    //        _baselog.log("UserName", "rmsmech@gmail.com", is_sub: true);
    //        _baselog.log("Pass", "hello@123", is_sub: true);

    //        //Exception
    //        try
    //        {
    //            int i = 5;
    //            int y = i / (i - i);
    //        }
    //        catch (Exception ex)
    //        {
    //            _baselog.log(ex, "some differernt exception at start");
    //            int i = 5 + 5;
    //        }
    //    }
    //}

    #endregion

    #region Events Processing
    public class Program
    {
        static void customtest02(itemstosend obj)
        {
            // what
        }
        static void customtest0waht()
        {
            // what
        }
        static void customtest03(somethintosend obj)
        {
            //Who hoooo
        }

        public static void Main(string[] args)
        {
            var newss = new newSubscibe();

            Thread tc1 = new Thread(() =>
            {
                EventStore.Singleton.GetEvent<CustomEvent01>().subscribe(customeventtest);
            });

            Thread tc2 = new Thread(() =>
            {
                EventStore.Singleton.GetEvent<CustomEvent02>().subscribe(customtest02);
                EventStore.Singleton.GetEvent<CustomEvent01>().publish();
            });
            Thread tc3 = new Thread(() =>
            {
                EventStore.Singleton.GetEvent<CustomEvent03>().subscribe(customtest03);
                EventStore.Singleton.GetEvent<CustomEvent01>().subscribe(customtest0waht);
                EventStore.Singleton.GetEvent<CustomEvent02>().publish(new itemstosend() { value = "This is a test" });
                EventStore.Singleton.clearAll(typeof(Program));
            });

            tc1.Start();
            tc2.Start();
            tc3.Start();

        }
        private static void customeventtest()
        {
            //this is for testing.
        }
    }

    public class CustomEvent01 : HEvent
    {

    }
    public class CustomEvent02 : HEvent<itemstosend>
    {

    }
    public class CustomEvent03 : HEvent<somethintosend>
    {

    }
    public class itemstosend
    {
        public string value { get; set; }
        public itemstosend() { }
    }
    public class somethintosend : EventArgs
    {
        public somethintosend() { }
    }

    public class newSubscibe
    {
        public newSubscibe() { EventStore.Singleton.GetEvent<CustomEvent03>().subscribe(helloworld); }

        private void helloworld(somethintosend obj)
        {
            //Do nothing.
        }
    }
    #endregion

    #region RuleEngine Processing
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        RuleEngine<PersonModel> _personRE = new RuleEngine<PersonModel>();
    //        //Rule is made of rule block and each rule block contains axioms
    //        var _rules_2 = makeRules<PersonModel>();
    //        //var _expressions_2 = RuleEngine.CompileRules<PersonModel>(_rules_2);
    //        _personRE.Compile(_rules_2);

    //        List<PersonModel> _pmlist = new List<PersonModel>();
    //        PersonModel _sengmodel = new PersonModel() { name = "Senguttuvan" };
    //        PersonModel _bhd = new PersonModel() { name = "bhadri" };
    //        PersonModel _bhadri = new PersonModel() { name = "Bhadri" };
    //        PersonModel _bhadri2 = new PersonModel() { name = "hello world" };
    //        PersonModel _bhadri3 = new PersonModel() { name = "Welcome World" };
    //        PersonModel _bhadri4 = new PersonModel() { name = "56" };

    //        _pmlist.Add(_sengmodel);
    //        _pmlist.Add(_bhd);
    //        _pmlist.Add(_bhadri);
    //        _pmlist.Add(_bhadri2);
    //        _pmlist.Add(_bhadri3);
    //        _pmlist.Add(_bhadri4);

    //        foreach (var _pm in _pmlist)
    //        {
    //            //RuleEngine.ProcessRules<PersonModel>(_pm, ref _expressions_2);
    //            _personRE.ProcessRules(_pm);
    //        }

    //        ////Rule is made of rule block and each rule block contains axioms
    //        //var _rules = makeRules<string>();
    //        //var _expressions = RuleEngine.CompileRules<string>(_rules);

    //        //List<string> _strlist = new List<string>();
    //        //_strlist.Add("hello world");
    //        //_strlist.Add("Welcome hello world");
    //        //_strlist.Add("Welcome World"); 
    //        //_strlist.Add("what a turn of events");
    //        //_strlist.Add("Welcome senguttuvan to this world");
    //        //_strlist.Add("5");

    //        //foreach (var _str in _strlist)
    //        //{
    //        //   RuleEngine.ProcessRules<string>(_str,ref _expressions);
    //        //}
    //    }

    //    private static List<Rule> makeRules<T>()
    //    {
    //        List<Rule> _rules = new List<Rule>();

    //        //RULES
    //        Rule _rule1 = new Rule("Base Rule 1");
    //        _rule1.description = "This is to check how the engine works";

    //        Rule _rule2 = new Rule("Rule 2");
    //        _rule2.description = "This is to check how the engine works";

    //        Rule _rule3 = new Rule("Rule 3");
    //        _rule3.description = "This is to check how the engine works";

    //        Rule _rule4 = new Rule("Rule 4");
    //        _rule4.description = "This is to check how the engine works";

    //        //AXIOMS
    //        IAxiom _axiom1 = new BinaryAxiom(AxiomOperator.Contains, "hello world");
    //        IAxiom _axiom2 = new BinaryAxiom(AxiomOperator.StartsWith, "Welcome");
    //        IAxiom _axiom3 = new BinaryAxiom(AxiomOperator.NotContains, "hello");
    //        IAxiom _axiom4 = new BinaryAxiom(AxiomOperator.EndsWith, "world");
    //        IAxiom _axiom5 = new PropertyAxiom<T>(AxiomOperator.Equals, "name", "Senguttuvan", getProperty);
    //        IAxiom _axiom6 = new PropertyAxiom(AxiomOperator.Equals, "name", "Senguttuvan") { ignore_case = false };

    //        AxiomAction<T> _customValidation = (T target, object[] args) => {
    //                return new AxiomResponse(ActionStatus.Pass, "This is just a custom check");
    //            };
    //        IAxiom _axiom7 = new MethodAxiom<T>(_customValidation, "TestMethod", "This is just a test","sengu",2,3,5.6,false) { ignore_case = false };
    //        IAxiom _axiom8 = new PropertyAxiom(AxiomOperator.GreaterThan,"name", "10");
    //        IAxiom _axiom9 = new PropertyAxiom(AxiomOperator.LessThan,"name", "50");

    //        //ASSIGN AXIOMS TO RULES
    //        _rule1.block = new RuleBlock(LogicalOperator.Or);
    //        _rule1.block.add(_axiom1);
    //        _rule1.block.add(_axiom2);
    //        _rules.Add(_rule1);

    //        _rule2.block = new RuleBlock();
    //        _rule2.block.add(_axiom3);
    //        _rule2.block.add(_axiom4);
    //        _rules.Add(_rule2);

    //        _rule3.block = new RuleBlock(LogicalOperator.Or);
    //            RuleBlock _subblock = new RuleBlock();
    //            _subblock.add(_axiom5);
    //            _subblock.add(_axiom6);

    //        _rule3.block.add(_subblock);
    //        _rule3.block.add(_axiom7);
    //        _rules.Add(_rule3);

    //        _rule4.block = new RuleBlock();
    //        _rule4.block.add(_axiom8);
    //        _rule4.block.add(_axiom9);
    //        _rules.Add(_rule4);

    //        return _rules;
    //    }

    //    public static object getProperty<T>(T target,string property_name)
    //    {
    //        if (target is PersonModel)
    //        {
    //          var _item =  Convert.ChangeType(target, typeof(PersonModel));
    //            return ((PersonModel)_item).name;
    //        }
    //        return "Senguttuvan";
    //    }

    //}

    //public class PersonModel
    //{
    //    public string name { get; set; }
    //    public override string ToString()
    //    {
    //        return name;
    //    }
    //    public PersonModel() { }
    //}
    #endregion

    #region MVVM
    //public class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        var _di = ContainerStore.Singleton.DI;
    //        NormalPower _np = new NormalPower() { name = "Silvester Stallone" };
    //        NormalPower _np2 = new NormalPower() { name = "James Bond" };
    //        NormalPower _np3 = new NormalPower() { name = "Rajinikanth" };
    //        NormalPower _np4 = new NormalPower() { name = "Rock" };
    //        NormalPower _np1 = new NormalPower() { name = "Vijayakanth" };
    //        SuperPower _sp5 = new SuperPower() { name = "John cena" };
    //        _di.Register<IPower, NormalPower>(_np);
    //        _di.RegisterWithKey<IPower, NormalPower>("James", _np2);
    //        _di.RegisterWithKey<IPower, NormalPower>("Rajini", _np3);
    //        _di.RegisterWithKey<IPower, NormalPower>("Rock", _np4);
    //        _di.RegisterWithKey<IPower, SuperPower>("Helloworld", _sp5);
    //        _di.Register<IPower, NormalPower>(_np1);
    //        var _person1 = _di.Resolve<Person>(); //Should return SS
    //        var _person2 = _di.Resolve<Person>("Rajini"); //Should return SS
    //        var _person3 = _di.Resolve<Person>("Rock"); //Should return SS
    //        var _allset = _di.Resolve<List<IPower>>(ResolveMode.Transient);
    //        var _allset2 = _di.Resolve<IPower[]>();
    //        var allPersons = _di.Resolve(typeof(List<IPower>));
    //        var newpersons = _di.Resolve<Newperson>();

    //        //Should return SS
    //        //Console.WriteLine(_person1._power.name);
    //        //var _person2 = _di.ResolveTransient<Person>(TransientCreationLevel.Current); //Should return SS. Since, targetonly ensures that the target "Person" is created as instance where as it's dependencies are not.
    //        //Console.WriteLine(_person2._power.name);
    //        //var _person3 = _di.ResolveTransient<Person>(TransientCreationLevel.CascadeAll); //Should return batman
    //        //Console.WriteLine(_person3._power.name);
    //        //MappingProviderBase _mpb = new MappingProviderBase();
    //        //_mpb.Add<IPower, SuperPower>(new SuperPower(), target: InjectionTarget.All);
    //        //var _person4 = _di.ResolveTransient<Person>(_mpb, MappingLevel.CascadeAll); //Should return batman
    //        //Console.WriteLine(_person4._power.name);
    //        //var _person5 = _di.ResolveTransient<Person>(_mpb, MappingLevel.CascadeAll); //Should return batman
    //        //Console.WriteLine(_person5._power.name);
    //    }
    //}
    //public class Person
    //{
    //    public int count { get; set; }
    //    public string name { get; set; }
    //    [HaleyInject]
    //    public IPower _power { get; set; }
    //    public Person(IPower newpower) { _power = newpower; }
    //}
    //public class Newperson
    //{
    //    public IPower[] _powers { get; set; }
    //    public List<IPower> _powers2 { get; set; }
    //    public Newperson(IPower[] powers, List<IPower> powers2) { _powers = powers;_powers2 = powers2; }
    //}
    //public interface IPower
    //{
    //    string name { get; set; }
    //}
    //public class NormalPower : IPower
    //{
    //    public string name { get; set; }
    //    public NormalPower() { name = "Batman"; }
    //}
    //public class SuperPower : IPower
    //{
    //    public int life_count { get; set; }
    //    public string name { get; set; }
    //    public SuperPower() { life_count = 5; name = "Superman"; }
    //}
    #endregion

}

