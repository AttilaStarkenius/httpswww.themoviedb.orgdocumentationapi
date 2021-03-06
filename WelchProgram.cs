using System;
namespace httpdeveloper.marvel.comdocsAPIMVCnewestversion
{
  class WelchProgram
  {
    static void Main(string[] args)
    {
      Console.WriteLine("\nBegin Welch's t-test using C# demo\n");

      Console.WriteLine(Student(2.0, 5));

      //int df = 5;
      //for (double t = -4.0; t <= 4.1; t += 0.1)
      //{
      //  Console.WriteLine(t.ToString("F1") + " " + StudentProbDensityFunc(t, df).ToString("F6"));
      //}

      //double[] t = new double[] { 0.0, 1.0, 1.5, 2.0, 2.5 };
      //int[] df = new int[] { 5, 10 };

      //for (int i = 0; i < t.Length; ++i)
      //{
      //  for (int j = 0; j < df.Length; ++j)
      //  {
      //    double p = Student(t[i], df[j]);
      //    Console.WriteLine(t[i] + " " + df[j] + " " + p.ToString("F6"));
      //  }

      //}

      //double p = Student(t, df);
      //Console.WriteLine(p.ToString("F6"));

      Console.ReadLine();

      //var x = new double[] { 78, 87, 78, 85, 80, 92, 88, 78, 90 };
      //var y = new double[] { 82, 76, 63, 71, 75, 70, 80, 81 };

      //Console.WriteLine("\nThe first data set is:\n");
      //ShowVector(x, 0);

      //Console.WriteLine("\nThe second data set is:\n");
      //ShowVector(y, 0);

      //Console.WriteLine("\nStarting Welch's t-test using C#\n");
      //TTest(x, y);

      Console.WriteLine("\nEnd t test demo\n");
      Console.ReadLine();
    } // Main

    static double StudentProbDensityFunc(double t, int df)
    {
      // see https://en.wikipedia.org/wiki/Student%27s_t-distribution

      // calc leading term
      double lead = 0.0;
      if (df % 2 == 0) // df is even
      {
        int num = 1;
        for (int term = 3; term <= df - 1; term += 2)
        {
          num = num * term;
        }
        //Console.WriteLine("num = " + num);

        int tmpDenom = 1;
        for (int term = 2; term <= df - 2; term += 2)
        {
          tmpDenom = tmpDenom * term;
        }
        double denom = 2.0 * Math.Sqrt(df) * tmpDenom;
        lead = num / denom;
        //Console.WriteLine("denom = " + denom);
      }
      else // df is odd
      {
        int num = 1;
        for (int term = 2; term <= df - 1; term += 2)
        {
          num = num * term;
        }

        int tmpDenom = 1;
        for (int term = 3; term <= df - 2; term += 2)
        {
          tmpDenom = tmpDenom * term;
        }
        double denom = Math.PI * Math.Sqrt(df) * tmpDenom;
        lead = num / denom;
      }

      // calculate the trail part
      double front = 1.0 + ((t * t) / df);
      double exponent = -1.0 * (df + 1) / 2.0;
      double tail = Math.Pow(front, exponent);

      return lead * tail;

    } // StudentProbDensityFunc

    static void ShowVector(double[] v, int dec)
    {
      for (int i = 0; i < v.Length; ++i)
        Console.Write(v[i].ToString("F" + dec) + " ");
      Console.WriteLine("");
    }

    public static void TTest(double[] x, double[] y)
    {
      // Welch's t-test for two samples
      int n1 = x.Length;
      int n2 = y.Length;

      double sumX = 0.0; // calculate means
      double sumY = 0.0;

      for (int i = 0; i < n1; ++i)
        sumX += x[i];

      for (int i = 0; i < n2; ++i)
        sumY += y[i];

      double meanX = sumX / n1;
      double meanY = sumY / n2;

      double sumXminusMeanSquared = 0.0; // calculate variances
      double sumYminusMeanSquared = 0.0;

      for (int i = 0; i < n1; ++i)
        sumXminusMeanSquared += (x[i] - meanX) * (x[i] - meanX);

      for (int i = 0; i < n2; ++i)
        sumYminusMeanSquared += (y[i] - meanY) * (y[i] - meanY);

      double varX = sumXminusMeanSquared / (n1 - 1);
      double varY = sumYminusMeanSquared / (n2 - 1);

      // t statistic
      double top = (meanX - meanY);
      double bot = Math.Sqrt((varX / n1) + (varY / n2));
      double t = top / bot;
      Console.WriteLine("t = " + t.ToString("F4"));

      // df mildly tricky
      // http://www.statsdirect.com/help/default.htm#parametric_methods/unpaired_t.htm
      double num = ((varX / n1) + (varY / n2)) *
        ((varX / n1) + (varY / n2));
      double denomLeft = ((varX / n1) * (varX / n1)) / (n1 - 1);
      double denomRight = ((varY / n2) * (varY / n2)) / (n2 - 1);
      double denom = denomLeft + denomRight;
      double df = num / denom;
      Console.WriteLine("df = " + df.ToString("F3"));

      double p = Student(t, df); // cumulative two-tail density
      Console.WriteLine("p-value = " + p.ToString("F6"));

      Console.WriteLine("mean of x = " + meanX.ToString("F2"));
      Console.WriteLine("mean of y = " + meanY.ToString("F2"));

      Explain();
    }

    public static void Explain()
    {
      Console.Write("\nThe p-value is the probability that ");
      Console.WriteLine("the two means are equal.\n");
      Console.Write("If the p-value is low, typically less ");
      Console.WriteLine("than 0.05 or 0.01, then you");
      Console.Write("conclude there is statistcal evidence ");
      Console.WriteLine("the two means are different.");
      Console.Write("But if the p-value isn't below the 5% ");
      Console.WriteLine("or 1% critical value then you");
      Console.Write("conclude there's not enough evidence to ");
      Console.WriteLine("say the means are different.\n");
    }

    //public static double Student(double t, object df)
    //{
    //  if (df.GetType() == typeof(double))
    //    return 1.0;
    //  else if (df.GetType() == typeof(int))
    //    return 2.0;
    //  return 3.0;
    //}

    public static double Student(double t, double df)
    {
      // for large integer df or double df
      // adapted from ACM algorithm 395
      // returns 2-tail p-value
      double n = df; // to sync with ACM parameter name
      double a, b, y;

      t = t * t;
      y = t / n;
      b = y + 1.0;
      if (y > 1.0E-6) y = Math.Log(b);
      a = n - 0.5;
      b = 48.0 * a * a;
      y = a * y;

      y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
        (0.8 * y * y + 100.0 + b) + y + 3.0) / b + 1.0) *
        Math.Sqrt(y);
      return 2.0 * Gauss(-y); // ACM algorithm 209
    } 

    public static double Student(double t, int df)
    {
      // adapted from ACM algorithm 395
      // for small integer df, or int df with large t
      int n = df; // to sync with ACM parameter name
      double a, b, y, z;

      z = 1.0;
      t = t * t;
      y = t / n;
      b = 1.0 + y;

      if (n >= 20 && t < n || n > 200) // moderate df and smallish t  ( df >= 20, t < df ) or very large df
      {
        double x = 1.0 * n; // make df a double
        return Student(t, x); // call double df version
      }

      if (n < 20 && t < 4.0)
      {
        a = Math.Sqrt(y);
        y = Math.Sqrt(y);
        if (n == 1)
          a = 0.0;
      }
      else
      {
        a = Math.Sqrt(b);
        y = a * n;
        for (int j = 2; a != z; j += 2)
        {
          z = a;
          y = y * (j - 1) / (b * j);
          a = a + y / (n + j);
        }
        n = n + 2;
        z = y = 0.0;
        a = -a;
      }

      int sanityCt = 0;
      while (true && sanityCt < 10000)
      {
        ++sanityCt;
        n = n - 2;
        if (n > 1)
        {
          a = (n - 1) / (b * n) * a + y;
          continue;
        }

        if (n == 0)
          a = a / Math.Sqrt(b);
        else // n == 1
          a = (Math.Atan(y) + a / b) * 0.63661977236; // 2/Pi

        return z - a;
      }

      return -1.0; // error
    } // Student (int df)



    // ==========================================================

    public static double Gauss(double z)
    {
      // input = z-value (-inf to +inf)
      // output = p under Normal curve from -inf to z
      // e.g., if z = 0.0, function returns 0.5000
      // ACM Algorithm #209
      double y; // 209 scratch variable
      double p; // result. called 'z' in 209
      double w; // 209 scratch variable

      if (z == 0.0)
        p = 0.0;
      else
      {
        y = Math.Abs(z) / 2;
        if (y >= 3.0)
        {
          p = 1.0;
        }
        else if (y < 1.0)
        {
          w = y * y;
          p = ((((((((0.000124818987 * w
            - 0.001075204047) * w + 0.005198775019) * w
            - 0.019198292004) * w + 0.059054035642) * w
            - 0.151968751364) * w + 0.319152932694) * w
            - 0.531923007300) * w + 0.797884560593) * y * 2.0;
        }
        else
        {
          y = y - 2.0;
          p = (((((((((((((-0.000045255659 * y
            + 0.000152529290) * y - 0.000019538132) * y
            - 0.000676904986) * y + 0.001390604284) * y
            - 0.000794620820) * y - 0.002034254874) * y
            + 0.006549791214) * y - 0.010557625006) * y
            + 0.011630447319) * y - 0.009279453341) * y
            + 0.005353579108) * y - 0.002141268741) * y
            + 0.000535310849) * y + 0.999936657524;
        }
      }

      if (z > 0.0)
        return (p + 1.0) / 2;
      else
        return (1.0 - p) / 2;
    } // Gauss()

  } // Program
} // ns
