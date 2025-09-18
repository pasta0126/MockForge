using MockForge.Core.Abstractions;
using System.Numerics;

namespace MockForge.Providers
{
    public sealed class ProgressionProvider(IRandomizer r) : IProvider
    {
        public string Name => "Progression";

        private readonly Dictionary<int, BigInteger> _cacheFibonacci = new();
        private readonly Dictionary<int, BigInteger> _cacheJacobsthal = new();
        private readonly Dictionary<int, BigInteger> _cacheLucas = new();
        private readonly Dictionary<int, BigInteger> _cachePell = new();
        private readonly Dictionary<int, BigInteger> _cacheHofstadterQ = new();
        private readonly Dictionary<int, BigInteger> _cacheExotic = new();

        public BigInteger Fibonacci(int n) => GetFibonacciBig(n);

        private BigInteger GetFibonacciBig(int n)
        {
            if (n <= 0) return 0;
            if (n == 1) return 1;
            if (_cacheFibonacci.TryGetValue(n, out BigInteger value))
                return value;

            if (n > 100)
            {
                BigInteger a = 0, b = 1;
                for (int i = 2; i <= n; i++)
                {
                    var temp = a + b;
                    a = b;
                    b = temp;
                    if (!_cacheFibonacci.ContainsKey(i))
                        _cacheFibonacci[i] = temp;
                }
                return b;
            }

            BigInteger result = GetFibonacciBig(n - 1) + GetFibonacciBig(n - 2);
            _cacheFibonacci[n] = result;
            return result;
        }

        public BigInteger Jacobsthal(int n) => GetJacobsthalBig(n);

        private BigInteger GetJacobsthalBig(int n)
        {
            if (n <= 0) return 0;
            if (n == 1) return 1;
            if (_cacheJacobsthal.TryGetValue(n, out BigInteger value))
                return value;

            if (n > 100)
            {
                BigInteger a = 0, b = 1;
                for (int i = 2; i <= n; i++)
                {
                    var temp = b + 2 * a;
                    a = b;
                    b = temp;
                    if (!_cacheJacobsthal.ContainsKey(i))
                        _cacheJacobsthal[i] = temp;
                }
                return b;
            }

            BigInteger result = GetJacobsthalBig(n - 1) + 2 * GetJacobsthalBig(n - 2);
            _cacheJacobsthal[n] = result;
            return result;
        }

        public BigInteger Lucas(int n) => GetLucasBig(n);

        private BigInteger GetLucasBig(int n)
        {
            if (n <= 0) return 2;
            if (n == 1) return 1;
            if (_cacheLucas.TryGetValue(n, out BigInteger value))
                return value;

            if (n > 100)
            {
                BigInteger a = 2, b = 1;
                for (int i = 2; i <= n; i++)
                {
                    var temp = a + b;
                    a = b;
                    b = temp;
                    if (!_cacheLucas.ContainsKey(i))
                        _cacheLucas[i] = temp;
                }
                return b;
            }

            BigInteger result = GetLucasBig(n - 1) + GetLucasBig(n - 2);
            _cacheLucas[n] = result;
            return result;
        }

        public BigInteger Pell(int n) => GetPellBig(n);

        private BigInteger GetPellBig(int n)
        {
            if (n <= 0) return 0;
            if (n == 1) return 1;
            if (_cachePell.TryGetValue(n, out BigInteger value))
                return value;

            if (n > 100)
            {
                BigInteger a = 0, b = 1;
                for (int i = 2; i <= n; i++)
                {
                    var temp = 2 * b + a;
                    a = b;
                    b = temp;
                    if (!_cachePell.ContainsKey(i))
                        _cachePell[i] = temp;
                }
                return b;
            }

            BigInteger result = 2 * GetPellBig(n - 1) + GetPellBig(n - 2);
            _cachePell[n] = result;
            return result;
        }

        public BigInteger HofstadterQ(int n) => GetHofstadterQBig(n);

        private BigInteger GetHofstadterQBig(int n)
        {
            if (n <= 2) return 1;
            if (_cacheHofstadterQ.TryGetValue(n, out BigInteger value))
                return value;
            
            int index1 = Math.Max(1, n - (int)GetHofstadterQBig(n - 1));
            int index2 = Math.Max(1, n - (int)GetHofstadterQBig(n - 2));
            BigInteger result = GetHofstadterQBig(index1) + GetHofstadterQBig(index2);
            _cacheHofstadterQ[n] = result;
            return result;
        }

        public double LogisticMap(int n, double r = 4.0, double x0 = 0.2)
        {
            if (n < 0) return x0;
            double x = x0;
            for (int i = 0; i < n; i++)
            {
                x = r * x * (1 - x);
            }
            return x;
        }

        public BigInteger Exotic(int n) => GetExoticBig(n);

        private BigInteger GetExoticBig(int n)
        {
            if (n <= 0) return 1;
            if (n == 1) return 1;
            if (_cacheExotic.TryGetValue(n, out BigInteger cachedValue))
                return cachedValue;
            
            int groupSize = 3;
            int group = n / groupSize;
            int sign = (group % 2 == 0) ? 1 : -1;
            BigInteger result = sign * (BigInteger.Abs(GetExoticBig(n - 1)) + BigInteger.Abs(GetExoticBig(n - 2)));
            _cacheExotic[n] = result;
            return result;
        }

        public BigInteger RandomFibonacci(int maxN = 20)
        {
            var n = r.Next(0, Math.Max(1, maxN + 1));
            return Fibonacci(n);
        }

        public BigInteger RandomJacobsthal(int maxN = 20)
        {
            var n = r.Next(0, Math.Max(1, maxN + 1));
            return Jacobsthal(n);
        }

        public BigInteger RandomLucas(int maxN = 20)
        {
            var n = r.Next(0, Math.Max(1, maxN + 1));
            return Lucas(n);
        }

        public BigInteger RandomPell(int maxN = 20)
        {
            var n = r.Next(0, Math.Max(1, maxN + 1));
            return Pell(n);
        }

        public BigInteger RandomHofstadterQ(int maxN = 20)
        {
            var n = r.Next(1, Math.Max(2, maxN + 1));
            return HofstadterQ(n);
        }

        public double RandomLogisticMap(int maxN = 100, double? rParam = null, double? x0 = null)
        {
            var n = r.Next(0, Math.Max(1, maxN + 1));
            var rValue = rParam ?? (r.NextDouble() * 3.0 + 1.0); // r entre 1.0 y 4.0
            var x0Value = x0 ?? r.NextDouble();
            return LogisticMap(n, rValue, x0Value);
        }

        public BigInteger RandomExotic(int maxN = 20)
        {
            var n = r.Next(0, Math.Max(1, maxN + 1));
            return Exotic(n);
        }

        public BigInteger RandomProgression(int maxN = 20)
        {
            var progressionType = r.Next(0, 6);
            
            return progressionType switch
            {
                0 => RandomFibonacci(maxN),
                1 => RandomJacobsthal(maxN),
                2 => RandomLucas(maxN),
                3 => RandomPell(maxN),
                4 => RandomHofstadterQ(maxN),
                5 => RandomExotic(maxN),
                _ => RandomFibonacci(maxN)
            };
        }

        public string GetProgressionName(int progressionType) => progressionType switch
        {
            0 => "Fibonacci",
            1 => "Jacobsthal", 
            2 => "Lucas",
            3 => "Pell",
            4 => "HofstadterQ",
            5 => "Exotic",
            _ => "Fibonacci"
        };

        public void ClearCache()
        {
            _cacheFibonacci.Clear();
            _cacheJacobsthal.Clear();
            _cacheLucas.Clear();
            _cachePell.Clear();
            _cacheHofstadterQ.Clear();
            _cacheExotic.Clear();
        }

        public void ClearCache(string progressionName)
        {
            switch (progressionName.ToLowerInvariant())
            {
                case "fibonacci": _cacheFibonacci.Clear(); break;
                case "jacobsthal": _cacheJacobsthal.Clear(); break;
                case "lucas": _cacheLucas.Clear(); break;
                case "pell": _cachePell.Clear(); break;
                case "hofstadterq": _cacheHofstadterQ.Clear(); break;
                case "exotic": _cacheExotic.Clear(); break;
            }
        }
    }
}