using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;

namespace MatrixTraversal
{
    [InheritedExport(typeof(IMatrixSoulution))]
    public interface IMatrixSoulution
    {
        List<(int, int)> Run(int[,] matrix, int m, int n);
    }

    public class MatrixTraversalChallenge : Challenge
    {
        [ImportMany(typeof(IMatrixSoulution), AllowRecomposition = true)]
        protected IMatrixSoulution[] matrixAlgos = null;

        public override void Run(IEnumerable<string> args)
        {
            if(args.Count() < 2)
            {
                Console.WriteLine("Invalid arguments: must specify matrix bounds");
                return;
            }

            var mSize = int.Parse(args.First());
            var nSize = int.Parse(args.Skip(1).First());
            if(mSize <= 0 || nSize <= 0)
            {
                Console.WriteLine("Invalid size specified: both sizes must be greater than 0.");
                return;
            }

            var random = new Random();
            var matrix = new int[mSize, nSize];
            matrix[0, 0] = 0;
            matrix[mSize - 1, nSize - 1] = 0;
            Console.WriteLine($"Testing MatrixTraversal algorithms with the following matrix");
            for(var m = 0; m < mSize; m++)
            {
                for (var n = 0; n < nSize; n++)
                {
                    if ((m == 0 && n == 0) || (m == mSize - 1 && n == nSize - 1))
                    { 
                        Console.Write($"\t{matrix[m, n]}");
                        continue;
                    }

                    matrix[m, n] = random.Next(0, 6);
                    if (matrix[m, n] == 0)
                    {
                        matrix[m, n] = -1;
                    }
                    Console.Write($"\t{matrix[m, n]}");
                }
                Console.WriteLine();
            }       

            Compose();
            var sw = new Stopwatch();
            foreach (var q in matrixAlgos)
            {
                List<(int, int)> result;
                try
                {
                    sw.Restart();
                    result = q.Run(matrix, mSize, nSize);
                    sw.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << !!! Threw exception with message: {ex.Message}");
                    continue;
                }

                var answer = "";
                var total = 0;
                if (result == null)
                {
                    answer = "no result possible";
                }
                else
                {
                    foreach(var r in result)
                    {
                        answer += $"{r} ";
                        total += matrix[r.Item1, r.Item2];
                    }
                }

                Console.WriteLine($"{q.GetType().Name} (in {sw.ElapsedMilliseconds} ms) << {total}points \n\tPath: {answer}");
            }
        }
    }
}
