using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serv
{
    class Program
    {
        static void Main(string[] args)
        {
            //DataMgr dataMgr = new DataMgr();
            ////注册
            //bool ret = dataMgr.Register("WSHC", "123");
            //if (ret)
            //{
            //    Console.WriteLine("注册成功");
            //}
            //else
            //{
            //    Console.WriteLine("注册失败");
            //}
            ////创建玩家
            //ret = dataMgr.CreatePlayer("WSHC");
            //if (ret)
            //{
            //    Console.WriteLine("创建玩家成功");
            //}
            //else
            //{
            //    Console.WriteLine("创建玩家失败");  
            //}
            ////获取玩家数据
            //PlayerData pd = dataMgr.GetPlayerData("WSHL");
            //if (pd!=null)
            //{
            //    Console.WriteLine("获取玩家数据成功 分数是 "+pd.score);
            //}
            //else
            //{
            //    Console.WriteLine("获取玩家数据失败");
            //}
            ////更改玩家数据
            //pd.score += 10;
            ////保存数据
            //Player p = new Player();
            //p.id = "WSHL";
            //p.data = pd;
            //dataMgr.Saveplayer(p);
            ////重新获取
            //pd = dataMgr.GetPlayerData("WSHL");
            //if (pd != null)
            //{
            //    Console.WriteLine("获取玩家数据成功 分数是 " + pd.score);
            //}
            //else
            //{
            //    Console.WriteLine("重新获取玩家数据失败");
            //}
            //Console.ReadLine();
            DataMgr dataMgr = new DataMgr();//只有实例化的对象才能使用单例模式(此框架中,其他实现instance的方式不同,情况不同)
            ServNet servNet = new ServNet();
            servNet.proto = new ProtocolBytes();
            servNet.Start("127.0.0.1", 1234);
            while (true)
            {
                string str = Console.ReadLine();
                switch (str)
                {
                    case "quit":
                        servNet.Close();
                        return;
                    case "print":
                        servNet.Print();
                        break;
                    default:
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
