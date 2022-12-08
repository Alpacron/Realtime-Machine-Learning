from fastapi import FastAPI
from starlette.middleware.cors import CORSMiddleware
from fastapi.responses import PlainTextResponse
from .controllers.object_recognition_controller import router as object_recognition_router

app = FastAPI(
    title="Object Recognition",
    description="Valorant Object Recognition.",
    version="1.0.0"
)

allowed_methods = ['POST', 'PUT', 'GET']

app.add_middleware(
    CORSMiddleware, allow_methods=allowed_methods, allow_headers=["*"])

app.include_router(object_recognition_router)


@app.get("/healthz")
def get_request():
    return PlainTextResponse("Healthy")
