using ILOG.Concert;
using ILOG.CPLEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VRP_VNS
{
    internal class Program
    {
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[100];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        static void Main(string[] args)
        {
            Program a = new Program();
            a.solver();

            if (1 == 0)
            {
                for (int count = 0; count < 20; count++)
                {
                    Console.WriteLine("----test{0}------", count);

                    #region Test
                    //Console.WriteLine(a.GetListIndex(3)[0]);
                    //Console.WriteLine(a.GetListIndex(3)[1]);
                    List<int> list1 = new List<int>() { 0, 1, 2, 3, 4, 5, 11 };
                    List<int> list2 = new List<int>() { 0, 6, 7, 8, 9, 10, 11 };

                    a.GetIndex(list1, list2);
                    //Console.WriteLine(a.GetIndex(list1, list2)[0]);
                    //Console.WriteLine(a.GetIndex(list1, list2)[1]);

                    //Console.WriteLine("Test exchange");
                    foreach (int i in list1)
                    {
                        Console.Write(i + ", ");
                    }
                    Console.WriteLine();
                    foreach (int i in list2)
                    {
                        Console.Write(i + ", ");
                    }
                    Console.WriteLine();

                    //a.ExchangeNode(a.GetIndex(list1, list2)[0], a.GetIndex(list1, list2)[1], list1, list2);
                    a.ExchangeNode2(a.GetIndex(list1, list2)[0], a.GetIndex(list1, list2)[1], list1, list2);
                    Console.WriteLine("After Test");
                    foreach (int i in list1)
                    {
                        Console.Write(i + ", ");
                    }
                    Console.WriteLine();
                    foreach (int i in list2)
                    {
                        Console.Write(i + ", ");
                    }
                    Console.WriteLine();

                    #endregion
                }
            }

        }



        //choose the lists
        public int[] GetListIndex(int K)
        {
            Random rnd = new Random(GetRandomSeed());
            int i = rnd.Next(K);
            int j = rnd.Next(K);
            int[] index = new int[2];
            while (i == j)
            {
                j = rnd.Next(K);
            }
            index[0] = i;
            index[1] = j;
            return index;
        }

        //get the index of the exchange index,only used if the length of list >0
        public int[] GetIndex(List<int> list1, List<int> list2)
        {
            int index1;
            int index2;
            int[] index = new int[2];
            Random rnd = new Random(GetRandomSeed());//0 bu bian,5 ok
            if (list1.Count > 2 && list2.Count > 2)
            {
                index1 = rnd.Next(1, list1.Count - 1);
                index2 = rnd.Next(1, list2.Count - 1);
                index[0] = index1;
                index[1] = index2;
            }

            else if (list1.Count == 2 && list2.Count > 2)
            {
                index1 = 1;
                index2 = rnd.Next(1, list2.Count - 1);
                index[0] = index1;
                index[1] = index2;
            }

            else if (list2.Count == 2 && list1.Count > 2)
            {
                index2 = 1;
                index1 = rnd.Next(1, list2.Count - 1);
                index[0] = index1;
                index[1] = index2;
            }
            else if (list1.Count == 2 && list2.Count == 2)
            {
                index1 = 1;
                index2 = 1;
                index[0] = index1;
                index[1] = index2;
            }
            return index;
        }

        public int[] GetInnerIndex(int len, List<int> list1)
        {
            int[] index = new int[2];
            if (list1.Count > 3)
            {
                Random rnd = new Random(GetRandomSeed());
                int i = rnd.Next(1, len - 1);
                int j = rnd.Next(1, len - 1);
                while (i == j)
                {
                    j = rnd.Next(1, len - 1);
                }
                index[0] = i;
                index[1] = j;
            }

            return index;

        }

        //两个list进行交换
        public void ExchangeNode(int index1, int index2, List<int> list1, List<int> list2)
        {
            if (list1.Count > 2 && list2.Count > 2)
            {
                if (index1 < list2.Count && index2 < list1.Count)
                {
                    int temp = list1[index1];
                    list1[index1] = list2[index2];
                    list2[index2] = temp;
                }
                else if (index1 > list2.Count)
                {
                    int temp = list2[index2];
                    list2.Insert(list2.Count - 2, list1[index1]);
                    list2.RemoveAt(index2);
                    list1.RemoveAt(index1);
                    list1[index1] = temp;

                }
                else if (index2 > list1.Count)
                {
                    int temp = list1[index1];
                    list1.Insert(list1.Count - 2, list2[index2]);//-1是最后一位
                    list1.RemoveAt(index1);
                    list2.RemoveAt(index2);
                    list2[index2] = temp;
                }
            }

            else if (list1.Count == 2 && list2.Count > 2)
            {
                int temp = list2[index2];
                list1.Insert(1, temp);
                list2.RemoveAt(index2);
            }

            else if (list1.Count > 2 && list2.Count == 2)
            {
                int temp = list1[index1];
                list2.Insert(1, temp);
                list1.RemoveAt(index1);
            }

        }

        //把一个list中的插入到另一个末尾
        public void ExchangeNode2(int index1, int index2, List<int> list1, List<int> list2)
        {
            if (list1.Count > 2 && list2.Count > 2)
            {
                int temp = list1[index1];
                list1.RemoveAt(index1);
                list2.Insert(index2, temp);
            }

            else if (list1.Count == 2 && list2.Count > 2)
            {
                list1.Insert(index1, list2[index2]);
                list2.RemoveAt(index2);
            }
            else if (list2.Count == 2 && list1.Count > 2)
            {
                list2.Insert(index1, list1[index1]);
                list1.RemoveAt(index1);

            }

        }

        //list内部交换

        public void ExchangeInnerNode(int index1, int index2, List<int> list)
        {
            int temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
        public void solver()
        {
            Cplex model = new Cplex();
            Random rnd = new Random(2);

            #region sets and parametes
            int N = 5;
            int K = 2;
            int M = 9999;
            int Q = 100;
            int[] q_n = new int[N + 2];
            int[][] d_nn = new int[N + 2][];
            int[] x_i = new int[N + 2];
            int[] y_i = new int[N + 2];
            int x_max = 10;
            int y_max = 10;
            List<int> node = new List<int>();
            for (int n = 1; n < N + 1; n++)
            {
                node.Add(n);
            }

            for (int n = 0; n < N + 2; n++)
            {
                if (n != 0 && n != N + 1)
                {
                    q_n[n] = rnd.Next(5, 30);
                    x_i[n] = rnd.Next(x_max);
                    y_i[n] = rnd.Next(y_max);
                }
                else
                {
                    q_n[n] = 0;
                    x_i[n] = 0;
                    y_i[n] = 0;

                }
            }




            for (int n = 0; n < N + 2; n++)
            {
                d_nn[n] = new int[N + 2];
                /*if (n >= 1 && n < N)//??
                {*/
                for (int m = 0; m < N + 2; m++)
                {
                    double x = (x_i[n] - x_i[m]) * (x_i[n] - x_i[m]);
                    double y = (y_i[n] - y_i[m]) * (y_i[n] - y_i[m]);
                    d_nn[n][m] = (int)Math.Abs(Math.Sqrt(x + y));
                }

                //}
            }
            #endregion

            for (int i = 0; i < d_nn.Length; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < d_nn[i].Length; j++)
                {
                    Console.Write("d_[{0}][{1}]={2} ", i, j, d_nn[i][j]);
                }
            }

            if (1 == 1)
            {
                #region devision variables



                INumVar[][] alpha = new INumVar[N + 2][];
                INumVar[][] gamma = new INumVar[N + 2][];
                INumVar[][][] beta = new INumVar[K][][];

                for (int n = 0; n < N + 2; n++)
                {
                    alpha[n] = new INumVar[N + 2];
                    for (int k = 0; k < K; k++)
                    {
                        alpha[n][k] = model.NumVar(0, 1, NumVarType.Bool);
                    }
                }

                for (int n = 0; n < N + 2; n++)
                {
                    gamma[n] = new INumVar[N + 2];
                    for (int k = 0; k < K; k++)
                    {
                        gamma[n][k] = model.NumVar(0, 9999, NumVarType.Float);
                    }
                }

                for (int k = 0; k < K; k++)
                {
                    beta[k] = new INumVar[N + 2][];
                    for (int n = 0; n < N + 2; n++)
                    {
                        beta[k][n] = new INumVar[N + 2];
                        for (int n1 = 0; n1 < N + 2; n1++)
                        {

                            beta[k][n][n1] = model.NumVar(0, n == n1 ? 0 : 1, NumVarType.Bool);
                        }
                    }

                }


                #endregion

                #region Obj



                INumExpr[] obj = new INumExpr[K];
                obj[0] = model.NumExpr();
                for (int k = 0; k < K; k++)
                {
                    for (int n = 0; n < N + 1; n++)
                    {
                        for (int n1 = 1; n1 < N + 2; n1++)
                        {
                            obj[0] = model.Sum(obj[0], model.Prod(d_nn[n][n1], beta[k][n][n1]));
                        }
                    }
                }
                model.AddMinimize(obj[0]);


                #endregion

                #region Subject

                // 1 node
                INumExpr[] expr1 = new INumExpr[K];
                for (int n = 1; n < N + 1; n++)
                {
                    expr1[0] = model.NumExpr();
                    for (int k = 0; k < K; k++)
                    {
                        expr1[0] = model.Sum(expr1[0], alpha[n][k]);
                    }

                    model.AddEq(expr1[0], 1);
                }

                //2 quantity

                INumExpr[] expr2 = new INumExpr[K];
                for (int k = 0; k < K; k++)
                {
                    expr2[0] = model.NumExpr();
                    for (int n = 1; n < N + 1; n++)
                    {
                        expr2[0] = model.Sum(expr2[0], model.Prod(q_n[n], alpha[n][k]));
                    }

                    model.AddLe(expr2[0], Q);
                }

                //flow
                INumExpr[] expr31 = new INumExpr[K];
                INumExpr[] expr32 = new INumExpr[K];

                for (int k = 0; k < K; k++)
                {
                    for (int n = 1; n < N + 1; n++)
                    {
                        expr31[0] = model.NumExpr();
                        expr32[0] = model.NumExpr();
                        for (int n1 = 0; n1 < N + 1; n1++)
                        {
                            expr31[0] = model.Sum(expr31[0], beta[k][n1][n]);
                        }
                        for (int n1 = 1; n1 < N + 2; n1++)
                        {
                            expr32[0] = model.Sum(expr32[0], beta[k][n][n1]);
                        }

                        model.AddEq(expr31[0], expr32[0]);
                        model.AddEq(expr31[0], alpha[n][k]);
                    }
                }


                //dummy first &last

                INumExpr[] expr41 = new INumExpr[K];
                INumExpr[] expr42 = new INumExpr[K];

                for (int k = 0; k < K; k++)
                {
                    expr41[0] = model.NumExpr();
                    expr42[0] = model.NumExpr();
                    for (int n = 0; n < N + 1; n++)
                    {
                        expr41[0] = model.Sum(expr41[0], beta[k][n][N + 1]);
                    }

                    for (int n = 1; n < N + 2; n++)
                    {
                        expr42[0] = model.Sum(expr42[0], beta[k][0][n]);
                    }

                    model.AddEq(expr41[0], expr42[0]);
                    model.AddEq(expr41[0], 1);
                }

                //sub

                INumExpr[] expr5 = new INumExpr[K];
                for (int k = 0; k < K; k++)
                {
                    for (int n = 0; n < N + 1; n++)
                    {
                        for (int n1 = 1; n1 < N + 2; n1++)
                        {
                            expr5[0] = model.NumExpr();
                            model.AddGe(gamma[n1][k], model.Diff(model.Sum(gamma[n][k], d_nn[n][n1]), model.Prod(M, model.Diff(1, beta[k][n][n1]))));
                        }
                    }
                }

                if (model.Solve())
                {
                    Console.WriteLine(model.ObjValue);
                    for (int k = 0; k < K; k++)
                    {
                        for (int n = 0; n < N + 1; n++)
                        {
                            for (int n1 = 1; n1 < N + 2; n1++)
                            {
                                if (model.GetValue(beta[k][n][n1]) > 0.9)
                                {
                                    try
                                    {
                                        Console.WriteLine("beta[{0}][{1}][{2}]={3}", k, n, n1, model.GetValue(beta[k][n][n1]));
                                        //Console.WriteLine("beta[{0}][{1}][{2}]={3},{4}", k, n, n1, model.GetValue(beta[k][n][n1]), model.GetValue(gamma[n][k]));
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                        Console.WriteLine();


                    }

                }

                #endregion
            }


            #region VNS

            int Iteration = 100;
            List<int>[] final_solution = new List<int>[K];


            //initialize solution
            List<int>[] ini_solution = new List<int>[K];//货车的初始解的长度不确定
            for (int i = 0; i < K; i++)
            {
                ini_solution[i] = new List<int>();
            }

            /*没有考虑到车的容量
             * for (int i = 1; i < N + 1; i++)
            {
                int sum_Q = 0;
                int index = rnd.Next(0, K);
                ini_solution[index].Add(i);                          
            }*/

            for (int k = 0; k < K; k++)
            {
                int sum_q = 0;
                while (node.Count > 0)
                {
                    //int index = rnd.Next(0, K);
                    int node_index = rnd.Next(0, node.Count);

                    sum_q += q_n[node[node_index]];
                    if (sum_q <= Q)
                    {
                        ini_solution[k].Add(node[node_index]);
                        //注意index的含义
                        //sum_q += q_n[node[node_index]];
                        node.Remove(node[node_index]);
                    }
                    else
                    {
                        break;//比sum大应该跳出循环而不是做其他的
                        sum_q -= q_n[node[node_index]];
                    }
                    //ini_solution[index].Add(node[node_index]);
                    //注意index的含义
                    //sum_q += q_n[node[node_index]];
                    //node.Remove(node[node_index]);
                    //Console.WriteLine("sum_q[{0}]={1}",k,sum_q);

                }

            }


            for (int i = 0; i < K; i++)
            {
                ini_solution[i].Insert(0, 0);
                ini_solution[i].Add(N + 1);
            }

            for (int i = 0; i < K; i++)
            {
                Console.WriteLine("Route{0}", i);
                foreach (int j in ini_solution[i])
                {
                    Console.Write(j + "-");
                }
                Console.WriteLine();

            }


            #region calculate ini-obj
            double result = 0;
            for (int k = 0; k < K; k++)
            {
                int i = 0; int j = 1;
                int index1;
                int index2;
                while (i < ini_solution[k].Count - 1)
                {
                    index1 = ini_solution[k][i];
                    index2 = ini_solution[k][j];
                    result += d_nn[index1][index2];
                    i++;
                    j++;
                }
            }
            Console.WriteLine(result.ToString());
            #endregion

            if (1 == 0)
            {
                #region calculate obj
                double tempResult = 0;
                for (int k = 0; k < K; k++)
                {
                    int pointer1 = 0; int pointer2 = 1;
                    int index1;
                    int index2;
                    while (pointer1 < ini_solution[k].Count - 1)
                    {
                        index1 = ini_solution[k][pointer1];
                        index2 = ini_solution[k][pointer2];
                        result += d_nn[index1][index2];
                        pointer1++;
                        pointer2++;
                    }
                }
                Console.WriteLine(result.ToString());
                #endregion
            }

            /* for (int count = 0; count < Iteration; count++)
             {
                 double tempResult = 0;
                 int[] IndexList = new int[2];
                 IndexList = GetListIndex(K);//choose list 

                 int[] IndexNode = new int[2];
                 IndexNode = GetIndex(ini_solution[IndexList[0]], ini_solution[IndexList[1]]);

                 //ExchangeNode(IndexNode[0], IndexNode[1], ini_solution[IndexList[0]], ini_solution[IndexList[1]]);
                 ExchangeNode(1, 1, ini_solution[IndexList[0]], ini_solution[IndexList[1]]);

                 #region calculate obj               
                 for (int k = 0; k < K; k++)
                 {
                     int pointer1 = 0; int pointer2 = 1;
                     int index1;
                     int index2;
                     while (pointer1 < ini_solution[k].Count - 1)
                     {
                         index1 = ini_solution[k][pointer1];
                         index2 = ini_solution[k][pointer2];
                         tempResult += d_nn[index1][index2];
                         pointer1++;
                         pointer2++;
                     }

                 }
                 if (tempResult < result)
                 {
                     result = tempResult;
                     final_solution = ini_solution;
                 }
                 //Console.WriteLine("temp={0}",tempResult);
                 // Console.WriteLine(result.ToString());


                 #endregion



             }*/

            for (int count = 0; count < Iteration; count++)
            {

                //Console.WriteLine("count={0}", count);
                double tempResult = 0;
                int list_index = rnd.Next(0, K);
                if (ini_solution[list_index].Count > 3)
                {
                    int[] InnderIndex = new int[2];

                    InnderIndex = GetInnerIndex(ini_solution[list_index].Count, ini_solution[list_index]);
                    ExchangeInnerNode(InnderIndex[0], InnderIndex[1], ini_solution[list_index]);

                    #region calculate obj               
                    for (int k = 0; k < K; k++)
                    {
                        int pointer1 = 0; int pointer2 = 1;
                        int index1;
                        int index2;
                        while (pointer1 < ini_solution[k].Count - 1)
                        {
                            index1 = ini_solution[k][pointer1];
                            index2 = ini_solution[k][pointer2];
                            tempResult += d_nn[index1][index2];
                            pointer1++;
                            pointer2++;
                        }

                    }
                    if (tempResult < result)
                    {
                        result = tempResult;
                        final_solution = ini_solution;
                    }
                    //Console.WriteLine("temp={0}",tempResult);
                    // Console.WriteLine(result.ToString());
                    #endregion
                }



            }


            Console.WriteLine(result.ToString());
            for (int i = 0; i < K; i++)
            {
                Console.WriteLine("Route{0}", i);
                foreach (int j in ini_solution[i])
                {
                    Console.Write(j + "-");
                }
                Console.WriteLine();

            }
            #endregion


        }

    }
}

