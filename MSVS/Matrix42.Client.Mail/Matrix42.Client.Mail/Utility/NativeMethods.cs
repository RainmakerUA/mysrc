using System;
using System.Runtime.InteropServices;

namespace Matrix42.Client.Mail.Utility
{
	internal static class Win32Api
	{
		private static class NativeMethods
		{
			/// <summary>
			/// No flags specified.Use default behavior for the function.
			/// </summary>
			public const uint FMFD_DEFAULT = 0x00000000;

			/// <summary>
			/// Treat the specified pwzUrl as a file name.
			/// </summary>
			public const uint FMFD_URLASFILENAME = 0x00000001;

			/// <summary>
			/// Internet Explorer 6 for Windows XPSP2 and later.
			/// Use MIME-type detection even if FEATURE_MIME_SNIFFING is detected.
			/// Usually, this feature control key would disable MIME-type detection.
			/// </summary>
			public const uint FMFD_ENABLEMIMESNIFFING = 0x00000002;

			/// <summary>
			/// Internet Explorer 6 for Windows XPSP2 and later.
			/// Perform MIME-type detection if "text/plain" is proposed,even if data sniffing is otherwise disabled.
			/// Plain text may be converted to text/html if HTML tags are detected.
			/// </summary>
			public const uint FMFD_IGNOREMIMETEXTPLAIN = 0x00000004;

			/// <summary>
			/// Internet Explorer 8. Use the authoritative MIME type specified in pwzMimeProposed.
			/// Unless FMFD_IGNOREMIMETEXTPLAIN is specified, no data sniffing is performed.
			/// </summary>
			public const uint FMFD_SERVERMIME = 0x00000008;

			/// <summary>
			/// Internet Explorer 9. Do not perform detection if "text/plain" is specified in pwzMimeProposed.
			/// </summary>
			public const uint FMFD_RESPECTTEXTPLAIN = 0x00000010;

			/// <summary>
			/// Internet Explorer 9. Returns image/png and image/jpeg instead of image/x-png and image/pjpeg.
			/// </summary>
			public const uint FMFD_RETURNUPDATEDIMGMIMES = 0x00000020;

			[DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
			public static extern int FindMimeFromData(IntPtr pBC,
														[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
														[MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1, SizeParamIndex=3)]
														byte[] pBuffer,
														int cbSize,
														[MarshalAs(UnmanagedType.LPWStr)]string pwzMimeProposed,
														uint dwMimeFlags,
														out IntPtr ppwzMimeOut,
														int dwReserved);

		}

		public static string FindMimeFromData(byte[] data, string mimeProposed)
		{
			var zero = IntPtr.Zero;
			var outMime = zero;

			var result = NativeMethods.FindMimeFromData(
													zero, null, data, data.Length, mimeProposed,
													NativeMethods.FMFD_RETURNUPDATEDIMGMIMES, out outMime, 0
												);

			if (result != 0)
			{
				throw Marshal.GetExceptionForHR(result);
			}

			var mimeResult = Marshal.PtrToStringUni(outMime);
			Marshal.FreeCoTaskMem(outMime);

			return mimeResult;
		}
	}
}
