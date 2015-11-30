from flask import Flask, g, render_template, request, redirect, url_for, abort, session
from flask_login import LoginManager, UserMixin, login_user, logout_user, login_required
from loremipsum import get_paragraph, get_sentence
import urllib, hashlib
from datetime import datetime

app = Flask(__name__)
app.secret_key = 'onueonuhnn345324!jmN]LWX/,?RT'


login_manager = LoginManager()
login_manager.init_app(app)


USERS = [
    {"id": 0, "is_public": True, "password": "123456", "country": "Vietnam", "region": "Hanoi", "display_name": "Trung Ngo", "email": "ndtrung4419@gmail.com"},
    {"id": 1, "is_public": True, "password": "123456", "country": "Vietnam", "region": "Ho Chi Minh City", "display_name": "Duc Minh", "email": "ducminhpham@gmail.com"},
    {"id": 2, "is_public": True, "password": "123456", "country": "USA", "region": "New York", "display_name": "Linh Mai", "email": "linhmai.etc@gmail.com"},
]

MESSAGES = [
    {"id": 0, "topic_id": 0, "author_id": 0, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 1, "topic_id": 0, "author_id": 1, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 2, "topic_id": 0, "author_id": 2, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 3, "topic_id": 0, "author_id": 0, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 4, "topic_id": 1, "author_id": 0, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 5, "topic_id": 1, "author_id": 1, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 6, "topic_id": 1, "author_id": 2, "content": get_paragraph(), "created_at": datetime.now()},
    {"id": 7, "topic_id": 1, "author_id": 0, "content": get_paragraph(), "created_at": datetime.now()},
]

TOPICS = [
    {"id": 0, "room_id": 0, "author_id": 0, "title": get_sentence(), "created_at": datetime.now()},
    {"id": 1, "room_id": 1, "author_id": 2, "title": get_sentence(), "created_at": datetime.now()}
]

ROOMS = [
    {"id": 0, "title": get_sentence(), "description": "This is a room for " + get_sentence(), "icon": "home"},
    {"id": 1, "title": get_sentence(), "description": "This is a room for " + get_sentence(), "icon": "eye"},
    {"id": 2, "title": get_sentence(), "description": "This is a room for " + get_sentence(), "icon": "fighter-jet"},
    {"id": 3, "title": get_sentence(), "description": "This is a room for " + get_sentence(), "icon": "folder"},
]

LOGIN_COUNT = 0


class LoginUser(UserMixin):
    def __init__(self, user):
        self.user = user

    def get_id(self):
        return unicode(self.user["id"])


@login_manager.user_loader
def load_user(user_id):
    user_id = int(user_id)
    if user_id < len(USERS):
        return LoginUser(USERS[user_id])


@app.before_request
def prepare_global_context():
    g.login_count = LOGIN_COUNT
    g.total_users = len(USERS)
    g.nav = {
        "in_forums_section": request.path == "/" or request.path.startswith("/topic") or request.path.startswith("/room"),
        "in_personal_profile": get_current_user_id() is not None and request.path == url_for('user_profile', user_id=get_current_user_id()),
        "in_user_search_section": request.path == url_for('user_search'),
    }
    print g.nav
    if 'user_id' in session:
        g.user = get_current_user()


def get_current_user_id():
    try:
        return int(session['user_id'])
    except:
        return None


def get_current_user():
    return get_user_by_id(get_current_user_id())


def get_user_by_id(user_id):
    return transform_user(USERS[user_id])


def get_user_by_email(email):
    return next((transform_user(user) for user in USERS if user["email"] == email))


def transform_message(msg):
    msg["author"] = get_user_by_id(msg["author_id"])
    msg["topic"] = get_topic_by_id(msg["topic_id"])
    return msg


def find_topics_in_room(room_id):
    return [transform_topic(t) for t in TOPICS if t["room_id"] == room_id]


def transform_room(room):
    room["topics"] = find_topics_in_room(room["id"])
    return room


def transform_topic(topic):
    topic["author"] = get_user_by_id(topic["author_id"])
    topic["messages"] = find_messages_in_topic(topic["id"])
    return topic


def gravatar(email, size=40):
    gravatar_url = "http://www.gravatar.com/avatar/" + hashlib.md5(email.lower()).hexdigest() + "?"
    gravatar_url += urllib.urlencode({'s': str(size)})
    return gravatar_url


def transform_user(user):
    email = user["email"]
    user["avatar"] = gravatar(email, 40)
    return user


def get_message_by_id(id):
    return transform_message(MESSAGES[id])


def find_messages_in_topic(topic_id):
    return [transform_message(m) for m in MESSAGES if m["topic_id"] == topic_id]


def find_messages_by_user(user_id):
    return [transform_message(m) for m in MESSAGES if m['author_id'] == user_id]


def add_message(content, author_id):
    MESSAGES.append({})


def get_topic_by_id(topic_id):
    return TOPICS[topic_id]


#
# ROOMS
#


def get_all_rooms():
    return map(transform_room, ROOMS)


def get_room_by_id(room_id):
    return transform_room(ROOMS[room_id])


@app.route("/topic/<topic_id>")
def topic_detail(topic_id):
    topic_id = int(topic_id)

    if topic_id >= len(TOPICS):
        return abort(404)

    topic = get_topic_by_id(topic_id)

    context = {
        "topic": topic,
        "messages": find_messages_in_topic(topic_id)
    }

    return render_template("topic.jinja.html", **context)


@app.route("/rooms")
def room_list():
    context = {
        "rooms": get_all_rooms()
    }
    return render_template("room_list.jinja.html", **context)



@app.route("/room/<room_id>")
def room_detail(room_id):
    room_id = int(room_id)

    if room_id >= len(ROOMS):
        abort(404)

    room = get_room_by_id(room_id)
    context = {
        "room": room,
        "topics": sorted(room["topics"], key=lambda m: m["created_at"], reverse=True)
    }
    return render_template("room_detail.jinja.html", **context)


@app.route("/create_topic", methods=["POST"])
@login_required
def create_topic():
    subject = request.form["subject"]
    content = request.form["content"]
    room_id = int(request.form["room_id"])

    next_topic_id = len(TOPICS)
    TOPICS.append({
        "id": next_topic_id,
        "title": subject,
        "author_id": get_current_user_id(),
        "room_id": room_id,
        "created_at": datetime.now(),
        })

    next_message_id = len(MESSAGES)
    MESSAGES.append({
        "id": next_message_id,
        "content": content,
        "author_id": get_current_user_id(),
        "created_at": datetime.now(),
        "topic_id": next_topic_id,
        })

    return redirect(url_for("topic_detail", topic_id=next_topic_id))


@app.route("/create_message", methods=["POST"])
@login_required
def create_message():
    content = request.form["content"]
    topic_id = int(request.form["topic_id"])

    next_message_id = len(MESSAGES)
    MESSAGES.append({
        "id": next_message_id,
        "content": content,
        "author_id": get_current_user_id(),
        "created_at": datetime.now(),
        "topic_id": topic_id
        })

    return redirect(url_for("topic_detail", topic_id=topic_id) + "#reply-" + str(next_message_id))


@app.route("/profile/<user_id>", methods=["GET", "POST"])
def user_profile(user_id):
    user_id = int(user_id)
    if user_id >= len(USERS):
        abort(404)

    user = get_user_by_id(user_id)
    if not user['is_public'] and user_id != get_current_user_id():
        abort(401)

    if request.method == "GET":
        context = {
            "user": user,
            "big_avatar": gravatar(user['email'], 150),
            "messages_by_user": find_messages_by_user(user_id),
        }
        return render_template("user_profile.jinja.html", **context)
    else:
        is_public = request.form["is_public"] == "True"
        user['is_public'] = is_public
        return redirect(url_for('user_profile', user_id=user_id))


@app.route("/find-doctor")
def user_search():
    context = {}
    return render_template("user_search.jinja.html", **context)


@app.route("/login", methods=["GET", "POST"])
def login():
    email = request.form["email"]
    password = request.form["password"]
    next_url = request.args.get('next')

    try:
        user = get_user_by_email(email)

        if user["password"] == password:
            login_user(LoginUser(user))
            session['user'] = get_current_user()
            global LOGIN_COUNT
            LOGIN_COUNT += 1
            return redirect(next_url or url_for('index'))
    except:
        pass

    return "Nope."


@app.route("/")
def index():
    return room_list()


@app.route("/logout", methods=["POST"])
@login_required
def logout():
    logout_user()
    global LOGIN_COUNT
    LOGIN_COUNT -= 1
    if LOGIN_COUNT <= 0:
        LOGIN_COUNT = 0
    next_url = request.args.get('next')
    return redirect(next_url or url_for('index'))
