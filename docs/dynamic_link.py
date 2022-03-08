with open('index.rst', 'r') as file:
  filedata = file.read()

filedata = filedata.replace('/altunitytester/', '/altunitytester-dev/')
filedata = filedata.replace('/altunityinspector/', '/altunityinspector-dev/')

with open('index.rst', 'w') as file:
  file.write(filedata)
