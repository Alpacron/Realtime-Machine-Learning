from fastapi import APIRouter

router = APIRouter(
    responses={404: {"description": "Not found"}},
)


@router.get("/send-message")
async def send_message():
    return {"status": "ok"}
