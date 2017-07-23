Copy-Item -Path "{path}\bin" -Destination "{isspath}" -Recurse
Copy-Item -Path "{path}\*.svc" -Destination "{isspath}"
Copy-Item -Path "{path}\Web.config" -Destination "{isspath}"