using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormat
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> output = new List<string>();
            try
            {
#if DEBUG
                if (true)
#else
                if (args != null && args.Length > 0)
#endif
                {
                    string input = "";
#if DEBUG
                    string fileName = Console.ReadLine();
                    if(System.IO.File.Exists(fileName))
                        input = System.IO.File.ReadAllText(fileName);
#else
                    if (System.IO.File.Exists(args[0]))
                         input = System.IO.File.ReadAllText(args[0]);
#endif
                    else                    
                    {
                        Console.WriteLine("找不到檔案...");                        
                        Console.Read();
                        return;
                    }
                    //string input = System.IO.File.ReadAllText("test.json");
                    string Indentation = "";

                    if (args.Length > 1)
                    {
                        int count = 1;
                        if (args.Length > 2)
                            count = int.Parse(args[2].ToString());

                        if (args[1].ToString() == "-s")
                            Indentation = repeatString(" ", count);
                        else if (args[1].ToString() == "-t")
                            Indentation = repeatString("\t", count);
                    }
                    else
                        Indentation = "\t";
                    


                    int space = 0;
                    string temp = "";
                    string line = "";

                    string[] nextline = new string[] { "{", "[", "]", "}" };
                    for (int i = 0; i < input.Length; i++)
                    {
                        temp = input[i].ToString();
                        //起始與結尾
                        /*
                        if (i == 0 || i == input.Length - 1)
                        {
                            output.Add(temp);
                            space++;
                            continue;
                        }*/
                 
                        switch (temp)
                        {

                            case "{":
                            case "[":
                                if (line.Length > 0)
                                {
                                    line += temp;
                                    break;
                                }
                                output.Add(repeatString(Indentation, space) + $"{temp}");
                                space++;
                                break;
                            case "}":
                            case "]":
                                if (line.Length > 0)
                                {
                                    line += temp;
                                    break;
                                }
                                space--;
                                // } ] 遇到下個字為","時先加逗號再換行
                                //並跳過下個字
                                if (i + 1 != input.Length && input[i + 1].ToString() == ",")
                                {
                                    output.Add(repeatString( Indentation , space) + $"{temp},");
                                    i++;
                                }
                                else
                                    output.Add(repeatString( Indentation , space) + $"{temp}");
                                break;
                            default:
                                //遇到第一個"時 先加入縮排
                                if (line == "" && (temp == "\"" |  System.Text.RegularExpressions.Regex.IsMatch(temp,"[0-9]")) ) 
                                    line = repeatString( Indentation , space) + temp;
                                else
                                    line += temp;
                                //檢查 line是否有成對的" 且temp 為, 
                                //如果有代表整句已結束可換行
                                //如果沒有代表","只是字串中的字元
                                if (line.Replace("\\\\","").Replace("\\\"", "").Count(x => x == '\"') % 2 == 0 && temp == ",")
                                {
                                    output.Add(line);
                                    line = "";
                                    break;
                                }
                                //檢查下個字元是否為 { [ ] } 是則代表該換行
                                if (line.Replace("\\\\", "").Replace("\\\"", "").Count(x => x == '\"') % 2 == 0)
                                {
                                    for (int j = 0; j < nextline.Length; j++)
                                    {
                                        if (input[i + 1].ToString() == nextline[j])
                                        {
                                            output.Add(line);
                                            line = "";
                                            break;
                                        }
                                    }
                                }
                                break;
                        }//Switch End

                    }//For End
                     //測試用
                     //System.IO.File.WriteAllLines("test_Format.json", output);
                     //Console.WriteLine($"Generate test_Format.json Done！");
#if DEBUG
                    System.IO.File.WriteAllLines(System.IO.Path.GetFileNameWithoutExtension(fileName) + "_Format.json", output);
                    Console.WriteLine($"Generate {System.IO.Path.GetFileNameWithoutExtension(fileName)}_Format.json Done！");
                    Console.Read();
#else
                    System.IO.File.WriteAllLines(System.IO.Path.GetFileNameWithoutExtension(args[0]) + "_Format.json", output);
                    Console.WriteLine($"Generate {System.IO.Path.GetFileNameWithoutExtension(args[0])}_Format.json Done！");
#endif
                    
                }//If End                
                else
                {
                    Console.WriteLine("參數說明：\r\n\t" +
                            "(1)Json檔名\r\n\t" +
                            "(2)縮排字元-t -s\r\n\t" +
                            "(3)每次幾個縮排字元\r\n" +
                            "按下任意鍵結束......");
                    Console.Read();
                }
            }//Try End
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //System.IO.File.WriteAllLines("fail.json", output);             
            }
            
         }
        static string repeatString(string input , int times)
        {
            string output = "";
            for (int i = 0; i < times; i++)
                output += input;
            return output;

            //另一種方法
            // "".PadLeft(times,input)
        }
    }
 }

