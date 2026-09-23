"""Keep the pre-restructure URLs working.

Writes a small page at every old path listed in legacy-redirects.json that
sends the visitor to where that content lives now. The #anchor never reaches
the server, so the page resolves it in the browser against the anchor map;
that is what lets pages/commands.html#click land on the Click section of
reference/input-actions.html instead of the top of some page.
"""
import html
import json
import os
import re

from sphinx.util import logging

logger = logging.getLogger(__name__)

MAP_FILE = 'legacy-redirects.json'

STUB = """<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="robots" content="noindex">
<title>Page moved - {project}</title>
{canonical}<script>
(function () {{
  var anchors = {anchors};
  var fallback = {fallback};
  function go() {{
    var hash = location.hash.slice(1);
    try {{ hash = decodeURIComponent(hash); }} catch (e) {{}}
    var known = Object.prototype.hasOwnProperty.call(anchors, hash);
    var target = known ? anchors[hash] : fallback && fallback + location.hash;
    if (target) {{
      var parts = target.split('#');
      location.replace('{root}' + parts[0] + location.search + (parts[1] ? '#' + parts[1] : ''));
    }}
  }}
  window.addEventListener('hashchange', go);
  go();
}})();
</script>
{refresh}</head>
<body>
<p>This page moved when the documentation was reorganised. Its content now lives here:</p>
<ul>
{links}
</ul>
</body>
</html>
"""


def _ids(path):
    with open(path, encoding='utf-8') as f:
        return set(re.findall(r'\sid="([^"]+)"', f.read()))


def _nav_order(env):
    """Docnames in the order the sidebar lists them."""
    order, stack = [], [env.config.root_doc]
    while stack:
        doc = stack.pop()
        if doc not in order:
            order.append(doc)
            stack.extend(reversed(env.toctree_includes.get(doc, [])))
    return {doc: i for i, doc in enumerate(order)}


def write_stubs(app, exc):
    if exc or app.builder.format != 'html':
        return
    map_path = os.path.join(app.srcdir, MAP_FILE)
    if not os.path.exists(map_path):
        return
    with open(map_path, encoding='utf-8') as f:
        redirects = json.load(f)

    nav = _nav_order(app.env)
    ids = {}
    written = 0
    for old, entry in redirects.items():
        out = os.path.join(app.outdir, old)
        if old[:-len('.html')] in app.env.found_docs:
            logger.warning('%s: %s is a real page again, so its redirect was skipped', MAP_FILE, old)
            continue

        targets = [entry['to']] if entry.get('to') else []
        targets += [t for t in entry['anchors'].values() if t not in targets]
        for target in targets:
            page, _, anchor = target.partition('#')
            built = os.path.join(app.outdir, page)
            if not os.path.exists(built):
                logger.warning('%s: %s points at %s, which was not built', MAP_FILE, old, page)
            elif anchor and anchor not in ids.setdefault(built, _ids(built)):
                logger.warning('%s: %s points at #%s, which %s no longer has', MAP_FILE, old, anchor, page)

        pages = sorted({t.partition('#')[0] for t in targets},
                       key=lambda page: nav.get(page[:-len('.html')], len(nav)))

        root = '../' * old.count('/')
        titles = app.env.titles
        links = []
        for page in pages:
            docname = page[:-len('.html')]
            title = titles[docname].astext() if docname in titles else page
            links.append('<li><a href="{}{}">{}</a></li>'.format(root, page, html.escape(title)))

        fallback = entry.get('to')
        os.makedirs(os.path.dirname(out), exist_ok=True)
        with open(out, 'w', encoding='utf-8') as f:
            f.write(STUB.format(
                project=html.escape(app.config.project),
                canonical='<link rel="canonical" href="{}{}">\n'.format(root, fallback) if fallback else '',
                anchors=json.dumps(entry['anchors'], separators=(',', ':')),
                fallback=json.dumps(fallback),
                root=root,
                refresh='<noscript><meta http-equiv="refresh" content="0; url={}{}"></noscript>\n'.format(
                    root, fallback) if fallback else '',
                links='\n'.join(links)))
        written += 1
    logger.info('wrote %d legacy redirect pages', written)


def setup(app):
    app.connect('build-finished', write_stubs)
    return {'parallel_read_safe': True, 'parallel_write_safe': True}
