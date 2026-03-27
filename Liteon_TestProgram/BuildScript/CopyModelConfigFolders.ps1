# 获取当前工作目录
$currentDirectory = $PWD.Path

# 获取上一层目录
#$parentDirectory = Split-Path -Path $currentDirectory -Parent

# 获取当前工作目录
$parentDirectory = $currentDirectory

Write-Host ($parentDirectory)

# 设置INI文件路径
$iniFilePath = Join-Path $parentDirectory "Model_Config\ShowSelectCaseName\ShowSelectCaseName.ini"

# 读取INI文件内容
$iniContent = Get-Content $iniFilePath | Out-String





# 使用正则表达式提取ProjectName的值
$regexPattern ='ProjectName\s*=\s*(.*)'
$projectNameMatch =[regex]::Match($iniContent, $regexPattern)


if ($projectNameMatch.Success) 
{
    # 如果我们知道会有多个由分号分隔的值，我们需要进一步处理这个字符串
    $allProjectNames =$projectNameMatch.Groups[1].Value.Split(';') -replace "`r|`n", ""
    #$allProjectNames = $allProjectNames + ";null"

    # 输出所有项目名称
    #foreach ($projectName in $allProjectNames)
    #{
        # 可能需要去除项目名称两侧的空格（如果有的话）
        # Write-Host ($projectName)
    #}
} 
else 
{
    Write-Host "No match found for ProjectName."
    return
}


Write-Host "INI Case: '$allProjectNames'"




  Write-Host "====================删除Model_Config文件夹========================="

   $targetPath_BulidFolder = Join-Path $parentDirectory "bin\LiteonProgram\Model_Config"
    # 删除文件夹及其所有内容
    Remove-Item -Path $targetPath_BulidFolder -Recurse -Force

    # 延迟 1 秒
    Start-Sleep -Seconds 1

    # 继续执行其他命令
    Write-Host "The original output folder has been deleted and has been delayed for 1 second."


Write-Host "===================复制VersionInfo文件=========================="

$sourceBasePath_VersionInfo = Join-Path $parentDirectory "VersionInfo.md"
$targetPath_VersionInfo = Join-Path $parentDirectory "bin\LiteonProgram\Tool_Config\VersionInfo.md"

Copy-Item -Path $sourceBasePath_VersionInfo -Destination $targetPath_VersionInfo -Force

Write-Host "===================复制ShowSelectCaseName文件夹=========================="

    # 复制ShowSelectCaseName
    $targetPath_ShowSelectCaseName = Join-Path $parentDirectory "bin\LiteonProgram\Model_Config"
    $sourceBasePath_ShowSelectCaseName = Join-Path $parentDirectory "Model_Config"

     $sourceFolderPath_ShowSelectCaseName = Join-Path $sourceBasePath_ShowSelectCaseName "ShowSelectCaseName"
     $targetFolderPath_ShowSelectCaseName = Join-Path $targetPath_ShowSelectCaseName "ShowSelectCaseName"

    
    if (Test-Path $sourceFolderPath_ShowSelectCaseName)
     {

        # 创建目标文件夹（如果不存在） 
        if (!(Test-Path -Path $targetFolderPath_ShowSelectCaseName)) 
        {     
            New-Item -ItemType Directory -Path $targetFolderPath_ShowSelectCaseName
        }

        # 复制文件夹内容
        Copy-Item -Path $sourceFolderPath_ShowSelectCaseName\* -Destination $targetFolderPath_ShowSelectCaseName -Recurse -Force
    } 
    else 
    {
        Write-Host "Source folder '$sourceFolderPath_ShowSelectCaseName' does not exist."
    }


    Write-Host "===================复制ZLogDataCollection文件夹=========================="

    # 复制ZLogDataCollection
    $targetPath_ZLogDataCollection = Join-Path $parentDirectory "bin\LiteonProgram\Model_Config"
    $sourceBasePath_ZLogDataCollection = Join-Path $parentDirectory "Model_Config"

     $sourceFolderPath_ZLogDataCollection = Join-Path $sourceBasePath_ZLogDataCollection "ZLogDataCollection"
     $targetFolderPath_ZLogDataCollection = Join-Path $targetPath_ZLogDataCollection "ZLogDataCollection"

    
    if (Test-Path $sourceFolderPath_ZLogDataCollection)
     {

        # 创建目标文件夹（如果不存在） 
        if (!(Test-Path -Path $targetFolderPath_ZLogDataCollection)) 
        {     
            New-Item -ItemType Directory -Path $targetFolderPath_ZLogDataCollection
        }

        # 复制文件夹内容
        Copy-Item -Path $sourceFolderPath_ZLogDataCollection\* -Destination $targetFolderPath_ZLogDataCollection -Recurse -Force
    } 
    else 
    {
        Write-Host "Source folder '$sourceFolderPath_ZLogDataCollection' does not exist."
    }

Write-Host "===================复制所有机种文件夹=========================="

      $substring = [string] "ALLCASE"
     if ($allProjectNames.ToLower().Contains($substring.ToLower()))
    {
       Write-Output "字符串包含 '$substring'"

             if (Test-Path $targetPath_ShowSelectCaseName)
            {

               # 创建目标文件夹（如果不存在） 
               if (!(Test-Path -Path $targetPath_ShowSelectCaseName)) 
               {     
                 New-Item -ItemType Directory -Path $targetPath_ShowSelectCaseName
               }

               # 复制文件夹内容
               Copy-Item -Path $sourceBasePath_ShowSelectCaseName\* -Destination $targetPath_ShowSelectCaseName -Recurse -Force

               Write-Host "All folders have been copied to the bulid directory."

              return
           } 
          else 
          {
            Write-Host "Source folder '$sourceBasePath_ShowSelectCaseName' does not exist."
             return
           }
     } 
     else 
    {
        Write-Output "字符串不包含 '$substring'"
    }

   
   


    Write-Host "====================复制指定机种的文件夹========================="


# 遍历每个项目名称，并复制对应的文件夹
foreach ($projectName in $allProjectNames) 
{
    $projectName = $projectName.Replace(" ", "")
    $projectName_Head = $projectName.Split('_')[0]
    
    
    # 设置源文件夹路径和目标文件夹路径
    # 构建目标路径和源路径
    $targetPath = Join-Path $parentDirectory "bin\LiteonProgram\Model_Config"
    $sourceBasePath = Join-Path $parentDirectory "Model_Config"

    $sourceFolderPath = Join-Path $sourceBasePath (Join-Path $projectName_Head $projectName)
    $targetFolderPath = Join-Path $targetPath (Join-Path $projectName_Head $projectName)

     #$sourceFolderPath = Join-Path $sourceBasePath $projectName_Head
     #$targetFolderPath = Join-Path $targetPath $projectName_Head

     Write-Host "机种大文件夹->"$projectName_Head
     Write-Host "机种小文件夹->"$projectName
     Write-Host "源文件->"$sourceFolderPath
     Write-Host "目标文件->"$targetFolderPath

    
    if (Test-Path $sourceFolderPath)
     {

        # 创建目标文件夹（如果不存在） 
        if (!(Test-Path -Path $targetFolderPath)) 
        {     
            New-Item -ItemType Directory -Path $targetFolderPath
        }

        # 复制文件夹内容
        Copy-Item -Path $sourceFolderPath\* -Destination $targetFolderPath -Recurse -Force

        
        Write-Host "Folders have been copied to the bulid directory."
    } 
    else 
    {
        Write-Host "Source folder '$sourceFolderPath' does not exist."
    }

    Write-Host "============================================="
}







