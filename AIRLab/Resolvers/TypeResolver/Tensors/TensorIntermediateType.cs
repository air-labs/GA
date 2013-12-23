using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{

    class TensorIntermediateType<T>
    {
        #region Списки классов
        public class TITRank1 : TensorIntermediateType<T> { }
        public class TITRank2 : TensorIntermediateType<T> { }
        public class TITRank3 : TensorIntermediateType<T> { }
        public class TITRank4 : TensorIntermediateType<T> { }
        public class TITRank5 : TensorIntermediateType<T> { }
        public class TITRank6 : TensorIntermediateType<T> { }
        public class TITRank7 : TensorIntermediateType<T> { }
        public class TITRank8 : TensorIntermediateType<T> { }
        public class TITRank9 : TensorIntermediateType<T> { }

        static List<Func<TensorIntermediateType<T>>> constructors = new List<Func<TensorIntermediateType<T>>>();

        static TensorIntermediateType()
        {
            constructors.Add(() => { throw new Exception("You can't create TIT Rank 0"); return null; });
            constructors.Add(() => new TITRank1());
            constructors.Add(() => new TITRank2());
            constructors.Add(() => new TITRank3());
            constructors.Add(() => new TITRank4());
            constructors.Add(() => new TITRank5());
            constructors.Add(() => new TITRank6());
            constructors.Add(() => new TITRank7());
            constructors.Add(() => new TITRank8());
            constructors.Add(() => new TITRank9());
        }

        public static int TITIndex(Type tit)
        {
            for (int i = 1; i < 10; i++) if (tit == GetTITType(i)) return i;
            return -1;
        }

        public static Type GetTITType(int rank)
        {
            if (rank == 1) return typeof(TensorIntermediateType<T>.TITRank1);
            if (rank == 2) return typeof(TensorIntermediateType<T>.TITRank2);
            if (rank == 3) return typeof(TensorIntermediateType<T>.TITRank3);
            if (rank == 4) return typeof(TensorIntermediateType<T>.TITRank4);
            if (rank == 5) return typeof(TensorIntermediateType<T>.TITRank5);
            if (rank == 6) return typeof(TensorIntermediateType<T>.TITRank6);
            if (rank == 7) return typeof(TensorIntermediateType<T>.TITRank7);
            if (rank == 8) return typeof(TensorIntermediateType<T>.TITRank8);
            if (rank == 9) return typeof(TensorIntermediateType<T>.TITRank9);
            throw new Exception("ranks greater than 9 are not supported");
        }

        public static TensorIntermediateType<T> CreateTIT(int rank)
        {
            var tit=constructors[rank]();
            tit.Rank = rank;
            return tit;

        }


        #endregion

        public Array CorrespondingArray { get; private set; }
        public int[] AccumumulatedAddress { get; private set; }
        public int Position { get; private set; }
        public int Rank { get; private set; }
       
        public int[] Resolve(int sub)
        {
            if (Rank != 1)
                throw new Exception("Can't resolve TIT, its rank not 1");
            var args = AccumumulatedAddress.ToArray();
            args[Position] = sub;
            return args;
        }

        public TensorIntermediateType<T> Next(int sub)
        {
            if (Rank == 1) throw new Exception("There is no Next TIT for Rank=1");
            var tit = CreateTIT(Rank - 1);
            tit.CorrespondingArray = CorrespondingArray;
            tit.AccumumulatedAddress = AccumumulatedAddress.ToArray();
            tit.AccumumulatedAddress[Position] = sub;
            tit.Position = Position + 1;
            

            return tit;
        }

        public static TensorIntermediateType<T> First(Array array, int sub)
        {
            var rank = array.GetType().GetArrayRank();
            if (rank == 1) throw new Exception("There is no Next TIT for Rank=1");
            var tit = CreateTIT(rank - 1);
            tit.CorrespondingArray = array;
            tit.AccumumulatedAddress = new int[rank];
            tit.AccumumulatedAddress[0] = sub;
            tit.Position = 1;
            return tit;
        }
    }
}
