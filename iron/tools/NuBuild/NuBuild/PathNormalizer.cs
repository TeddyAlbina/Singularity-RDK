﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace NuBuild
{
    class PathNormalizer
    {
        Dictionary<string, string> cache;
        Char[] directorySeparators;

        public PathNormalizer()
        {
            cache = new Dictionary<string, string>();
            directorySeparators = new Char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
        }

        private string cleanDotDots(string path)
        {
            string[] parts = path.Split(directorySeparators);
            List<string> outParts = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Equals(""))
                {
                    //- Null path segment: foo//bar.
                    continue;
                }
                else if (parts[i].Equals("."))
                {
                    //- semantically-null segment
                    continue;
                }
                else if (parts[i].Equals(".."))
                {
                    outParts.RemoveAt(outParts.Count() - 1);
                }
                else
                {
                    outParts.Add(parts[i]);
                }
            }
            return String.Join(Path.DirectorySeparatorChar.ToString(), outParts.ToArray());
        }

        //- Normalize the case of an absolute path to the case present in the filesystem
        public string normalizeAbsolutePath(string absPath)
        {
            string dotdotfreepath = cleanDotDots(absPath);
            if (!Path.IsPathRooted(dotdotfreepath))
            {
                throw new ArgumentException("Requires absolute path");
            }
            return normalizePath(dotdotfreepath, false);
        }
        
        //- invariant: input is an absolute path, free of ..s, lowercased, with
        //- normalized path separators.
        //- Based on suggestions in http://stackoverflow.com/questions/1214513/normalize-directory-names-in-c-sharp
        private string normalizePath(string requestPath, bool presumedDirectory)
        {
            string lowerPath = requestPath.ToLower();
            if (cache.ContainsKey(lowerPath))
            {
                return cache[lowerPath];
            }
            string rc = normalizePath_nocache(requestPath, presumedDirectory);
            cache[lowerPath] = rc;
            return rc;
        }

        public static string dbg_normalizePath_nocache(string requestPath, bool presumedDirectory)
        {
            return normalizePath_nocache(requestPath, presumedDirectory);
        }

        private static string normalizePath_nocache(string requestPath, bool presumedDirectory)
        {
            try
            {
                string rc;
                string childName = Path.GetFileName(requestPath);
                if (childName == "")
                {
                    //- absPath was a "root" (MSDOS drive letter)
                    //- by fiat, drive letters are uppercase.
                    rc = requestPath.ToUpper() + Path.DirectorySeparatorChar;
                }
                else
                {
                    string parentPath = Path.GetDirectoryName(requestPath);

                    //- Recurse to handle parent prefix:
                    string normalizedParent = normalizePath_nocache(parentPath, true);

                    DirectoryInfo parentDirectoryInfo = new DirectoryInfo(normalizedParent);
                    FileSystemInfo[] childrenFileSystemInfos = null;
                    string normalizedPath;
                    try
                    {
                        childrenFileSystemInfos = parentDirectoryInfo.GetFileSystemInfos(childName);
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        //- fall through and assume we're to create it.
                    }
                    if (childrenFileSystemInfos == null || childrenFileSystemInfos.Length == 0)
                    {
                        //- Looks like a nonexistent path. I guess the caller gets to decide the
                        //- capitalization. NB this is fraught with danger, since we're not actually
                        //- creating the path in the filesystem, so someone else might try to create
                        //- a path with a different capitalization. However, if we memoize our
                        //- results, we should end up canonicalizing to the first capitalization
                        //- we see.
                        normalizedPath = Path.Combine(normalizedParent, childName);
                        //- Unfortunately, we can't tell whether we should add a path separator here!
                        if (presumedDirectory)
                        {
                            normalizedPath += Path.DirectorySeparatorChar;
                        }
                    }
                    else
                    {
                        FileSystemInfo childFileSystemInfo = childrenFileSystemInfos.First();
                        //- Since we passed a normalized path into DirectoryInfo, we'll get
                        //- the normalized path back out, plus the filesystem's idea of the
                        //- child name's case.
                        normalizedPath = childFileSystemInfo.FullName;
                        if ((childFileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            normalizedPath += Path.DirectorySeparatorChar;
                        }
                    }
                    rc = normalizedPath;
                }
                //-Logger.WriteLine(String.Format("{0}\n  => {1}", requestPath, rc));
                return rc;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                throw new ArgumentException("invalid path");
            }
        }

    }
}
