using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;

namespace JenkinsServiceControl.Core
{
    public class FolderDelete
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //public 메소드
        // root 폴더(Upper Folder) 와 검색 폴더 이름을 받음
        //내부 메소드 getDirectories() 와 deleteDirectories() 호출

        public void deleteFolders(string upperDirectoryName, string directoryName)
        {
            //1. Get Directories
            var folderFound = GetDirectories(upperDirectoryName, directoryName).ToList();
            string result = null;

            if (folderFound.Count > 0)
            {
                printDirectories(folderFound);
                logger.Info(" Folder Found: " + folderFound.Count);
                Console.WriteLine(" Folder Found: " + folderFound.Count);
                result = DeleteDirectories(folderFound);                //2. Delete Directories
            }
            else
            {
                result = " Folder Not Found. " + directoryName + ". ";
            }

            Console.WriteLine(result);
            logger.Info(result);

        }

        // 폴더 검색
        // root 폴더(Upper Folder) 와 검색 폴더 이름으로 검색
        IEnumerable<string> GetDirectories(string upperDirectoryName, string directoryName)
        {
            var folderFound = Directory.GetDirectories(upperDirectoryName, directoryName, SearchOption.AllDirectories);
            return folderFound;
        }

        //Path list 에 있는 폴더 삭제
        string DeleteDirectories(IEnumerable<string> list)
        {

            string result = " Folders Found Deleted, ";
            string notDeleted = " and Not Deleted Paths = ";

            int deletedCounter = 0;
            int notDeletedCounter = 0;

            //1. Check Path, 2. Delete Folder
            foreach (var path in list)
            {
                if (CheckPath(path))
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    notDeleted = notDeleted + ", " + path;
                    notDeletedCounter = notDeletedCounter + 1;
                }
                deletedCounter++;
            }
            return result + deletedCounter + ", " + notDeleted + "= " + notDeletedCounter;
        }


        //Path 가 있는지 여부 확인
        bool CheckPath(string path)
        {
            Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            return r.IsMatch(path);
        }

        //확인 된 Directory 화면에 출력하기
        //testCode
        void printDirectories(IEnumerable<string> list)
        {
            foreach (var dir in list)
            {
                logger.Info(" Directory: " + dir);
                Console.WriteLine(" Directory: " + dir);
            }
        }

    }
}
