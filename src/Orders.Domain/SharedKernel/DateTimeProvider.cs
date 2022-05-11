﻿// using System;
//
// namespace SampleProject.Domain.SharedKernel
// {
//     public static class DateTimeProvider
//     {
//         private static DateTime? _customDate;
//
//         public static DateTime Now
//         {
//             get
//             {
//                 if (_customDate.HasValue)
//                 {
//                     return _customDate.Value;
//                 }
//
//                 return DateTime.UtcNow;
//             }
//         }
//
//         public static void Set(DateTime customDate) => _customDate = customDate;
//
//         public static void Reset() => _customDate = null;
//     }
// }