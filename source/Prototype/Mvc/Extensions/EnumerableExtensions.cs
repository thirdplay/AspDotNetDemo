using System;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// シーケンスの拡張機能を提供します。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// シーケンスを指定されたサイズのチャンクに分割します。
        /// </summary>
        /// <typeparam name="T">シーケンスの要素の型</typeparam>
        /// <param name="self">シーケンス</param>
        /// <param name="chunkSize">チャンクサイズ</param>
        /// <returns>チャンクサイズに分割したシーケンス</returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> self, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("Chunk size must be greater than 0.", nameof(chunkSize));
            }

            while (self.Any())
            {
                yield return self.Take(chunkSize);
                self = self.Skip(chunkSize);
            }
        }
    }
}