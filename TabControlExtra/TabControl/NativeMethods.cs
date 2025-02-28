﻿/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
*/

using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace TradeWright.UI.Forms 
{
    /// <summary>
    /// See https://docs.microsoft.com/en-gb/visualstudio/code-quality/ca1060-move-p-invokes-to-nativemethods-class?view=vs-2017
    /// </summary>
    internal sealed class NativeMethods
	{
		private NativeMethods(){}
		
#region Windows Constants

		public const int WM_GETTABRECT = 0x130a;
		public const int WS_EX_TRANSPARENT = 0x20;
		public const int WM_SETFONT = 0x30;
		public const int WM_FONTCHANGE = 0x1d;
		public const int WM_HSCROLL = 0x114;
		public const int TCM_HITTEST = 0x130D;
		public const int WM_PAINT = 0xf;
		public const int WS_EX_LAYOUTRTL  = 0x400000;
		public const int WS_EX_NOINHERITLAYOUT = 0x100000;

        #endregion

#region Misc Functions

		public static int LoWord(IntPtr dWord){
            return dWord.ToInt32() & 0xffff;
        }

        public static int HiWord(IntPtr dWord){
            if ((dWord.ToInt32() & 0x80000000) == 0x80000000)
                return (dWord.ToInt32() >> 16);
            else
                return (dWord.ToInt32() >> 16) & 0xffff;
        }
 
 		public static IntPtr ToIntPtr(object structure){
			IntPtr lparam;
			lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, lparam, false);
			return lparam;
		}
		

#endregion

#region Windows Structures and Enums

		[Flags()]
		public enum TCHITTESTFLAGS{
			TCHT_NOWHERE = 1,
			TCHT_ONITEMICON = 2,
			TCHT_ONITEMLABEL = 4,
			TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
		}
	
 
		
		[StructLayout(LayoutKind.Sequential)]
		public struct TCHITTESTINFO{
			
			public TCHITTESTINFO(Point location){
				pt = location;
				flags = TCHITTESTFLAGS.TCHT_ONITEM;
			}
			
			public Point pt;
			public TCHITTESTFLAGS flags;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct PAINTSTRUCT{
		    public IntPtr hdc;
		    public int fErase;
		    public RECT rcPaint;
		    public int fRestore;
		    public int fIncUpdate;
		    [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
		    public byte[] rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT{
		    public int _left;
		    public int _top;
		    public int _right;
		    public int _bottom;
		    
		    public RECT(int left, int top, int right, int bottom){
                _left = left;
		        _top = top;
		        _right = right;
		        _bottom = bottom;
		    }
		
		    public RECT(Rectangle r){
		        _left = r.Left;
		        _top = r.Top;
		        _right = r.Right;
		        _bottom = r.Bottom;
		    }
		
		    public static RECT FromXYWH(int x, int y, int width, int height){
		        return new RECT(x, y, x + width, y + height);
		    }
		
		    public static RECT FromIntPtr(IntPtr ptr){
		    	RECT rect = (RECT)Marshal.PtrToStructure(ptr, typeof(RECT));
		    	return rect;
		    }
		    
		    public readonly Size Size{
		        get{
		            return new Size(_right - _left, _bottom - _top);
		        }
		    }
		}
		

#endregion

	}
	
}
