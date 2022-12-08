from fastapi import FastAPI
from starlette.middleware.cors import CORSMiddleware

app = FastAPI(
    title="Object Recognition",
    description="Valorant Object Recognition.",
    version="1.0.0"
)

allowed_methods = ['POST', 'PUT', 'GET']

app.add_middleware(
    CORSMiddleware, allow_methods=allowed_methods, allow_headers=["*"])


@app.get("/")
def get_request():
    return "Hello World!"


@app.get("/messaging")
def get_request():
    return ""
