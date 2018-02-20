﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulu.Extensions
{
  public static class DateTimeExtensions
  {
    /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
    public static long ToUnixEpoch(this DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
  }
}