#!/bin/bash
if command -v podman &> /dev/null; then
  alias docker=podman
else
  echo "podman not installed"
fi

# 读取文件内容，并使用grep和awk提取Version的值
version=$(grep "<Version>" src/Directory.Build.props | awk -F'[<>]' '{print $3}')
appName=registry.cn-hangzhou.aliyuncs.com/hgui/opentask:$version
# 输出Version的值
echo "Version: $version"

docker build . -t $appName
if [ $? -ne 0 ]; then
    echo "docker image build fail 😭😭😭"
else
    echo "docker image build success 🎉🎉🎉"
fi

docker push $appName
read
exit 1