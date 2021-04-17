#pragma warning disable IDE0063
using Eto.Drawing;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace App.AdventureMaker.Core.Lib_Interop
{
	public static class PfimImage
	{
		public static Image FromFile(string filePath) => FromFile(new FileInfo(filePath));

		public static Image FromFile(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new FileNotFoundException(null, file.FullName);
			}

			try
			{
				return new Bitmap(file.FullName);
			}
			catch (Exception)
			{
				using (var image = Pfim.Pfim.FromFile(file.FullName))
				{
					GCHandle handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);


					try
					{
						int[] pixels = new int[image.Width * image.Height];
						int channels = image.DataLen / pixels.Length;

						unsafe
						{
							byte r, g, b, a = 255;
							fixed (byte* buffer = &image.Data[0])
							{
								for (int buffOffset = 0, index = 0; buffOffset < image.DataLen; buffOffset += channels, ++index)
								{
									b = *(buffer + buffOffset + 0);
									g = *(buffer + buffOffset + 1);
									r = *(buffer + buffOffset + 2);

									if (channels == 4)
									{
										a = *(buffer + buffOffset + 3);
									}

									pixels[index] =	(a << 24) | (r << 16) | (g << 8) | b;
								}
							}
						}

						return new Bitmap(image.Width, image.Height, PixelFormat.Format32bppRgba, pixels);
					}
					catch (Exception)
					{
						return null;
					}
					finally
					{
						handle.Free();
					}
				}
			}
		}
	}
}
